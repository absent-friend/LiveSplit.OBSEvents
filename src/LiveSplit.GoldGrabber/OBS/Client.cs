using LiveSplit.GoldGrabber.OBS.Protocol;
using LiveSplit.GoldGrabber.OBS.Protocol.Requests;
using LiveSplit.GoldGrabber.OBS.Protocol.Responses;
using LiveSplit.Web;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiveSplit.GoldGrabber.OBS
{
    internal class Client : IDisposable
    {
        private const int CLOSE_CODE_AUTHENTICATION_FAILED = 4009;
        private const int REQUEST_STATUS_OUTPUT_RUNNING = 500;
        private const int EVENT_SUBSCRIPTIONS_NONE = 0;
        
        private readonly Mutex _busyLock = new();
        private readonly Uri _uri;
        private readonly string _password;
        private readonly byte[] _receiveBuffer = new byte[4096];
        private ClientWebSocket _webSocket;
        private bool _disposed;

        internal Client(string host, int port, string password)
        {
            _uri = new Uri($"ws://{host}:{port}");
            _password = password;
        }

        public bool IsConnected => _webSocket != null;

        public async Task EstablishSession()
        {
            _busyLock.WaitOne();
            try
            {
                if (_webSocket != null)
                    return;
                
                _webSocket = new ClientWebSocket();
                try
                {
                    await _webSocket.ConnectAsync(_uri, CancellationToken.None);
                }
                catch (WebSocketException e)
                {
                    throw new ClientException("Unable to connect to OBS. Make sure that it's running and the websocket server is enabled.", e);
                }
                catch (Exception e)
                {
                    throw new ClientException($"Unexpected error connecting to OBS ({e.GetType().Name}). message: {e.Message}", e);
                }

                Hello helloMessage = await ReceiveMessage(Hello.Parse);
                if (helloMessage.Authentication != null && "".Equals(_password.Trim()))
                {
                    throw new ClientException("You must enter a password to connect to your OBS instance.");
                }

                string authResponse = null;
                if (helloMessage.Authentication != null)
                {
                    using SHA256 sha256 = SHA256.Create();
                    string saltedPass = _password + helloMessage.Authentication.Salt;
                    byte[] saltedPassHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPass));
                    string saltedPassBase64 = Convert.ToBase64String(saltedPassHash);

                    string challengeResponse = saltedPassBase64 + helloMessage.Authentication.Challenge;
                    byte[] challengeResponseHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(challengeResponse));
                    authResponse = Convert.ToBase64String(challengeResponseHash);
                }
                var identify = new Identify(helloMessage.RpcVersion, authResponse, EVENT_SUBSCRIPTIONS_NONE);
                await SendMessage(identify);
                // if we get an Identified message, we've successfully established a connection.
                await ReceiveMessage(Identified.Parse);

                // start the replay buffer if it isn't already started.
                StartReplayBufferResponse startBuffer = await SendRequest(new StartReplayBuffer());
                if (!startBuffer.RequestStatus.Result && startBuffer.RequestStatus.Code != REQUEST_STATUS_OUTPUT_RUNNING)
                {
                    throw new ClientException($"Failed to start the replay buffer: {startBuffer.RequestStatus.Comment}");
                }
            }
            catch (ClientException)
            {
                _webSocket.Dispose();
                _webSocket = null;
                throw;
            }
            catch (Exception e)
            {
                _webSocket.Dispose();
                _webSocket = null;
                throw new ClientException($"Unexpected exception ({e.GetType().Name}): {e.Message}", e);
            }
            finally
            {
                _busyLock.ReleaseMutex();
            }
        }

        public async Task SaveGoldSegmentReplay(string segmentName, TimeSpan segmentTime)
        {
            if (_webSocket == null || !_busyLock.WaitOne(TimeSpan.Zero))
            {
                return;
            }
            
            try
            {
                if (_uri.IsLoopback)
                {
                    // batch save + get filename so we can rename the file afterwards.
                    Message[] requests = [new SaveReplayBuffer(), new GetLastReplayBufferReplay()];
                    RequestBatchResponse response = await SendRequestBatch(new RequestBatch(requests));
                    if (response.Results.Count > 1)
                    {
                        GetLastReplayBufferReplayResponse lastReplay = GetLastReplayBufferReplayResponse.Transform(response.Results[1]);
                        string sanitizedSegmentName = FileUtils.StripInvalidFilenameChars(segmentName);
                        string goldTime = FileUtils.FormatTimeSpanForFilename(segmentTime);
                        string newReplayName = $"{sanitizedSegmentName}-{goldTime}";
                        try
                        {
                            FileUtils.Rename(lastReplay.SavedReplayPath, newReplayName);
                        }
                        catch (Exception e)
                        {
                            Logger.Warning($"Failed to rename replay file: {e.Message}");
                        }
                    } 
                    else
                    {
                        SaveReplayBufferResponse saveReplay = SaveReplayBufferResponse.Transform(response.Results[0]);
                        Logger.Warning($"Failed to save replay buffer: {saveReplay.RequestStatus.Comment ?? "The replay buffer isn't running."}");
                    }
                }
                else
                {
                    // just save the replay. can't rename a file on a remote server without additional dependencies/processes
                    SaveReplayBufferResponse saveReplay = await SendRequest(new SaveReplayBuffer());
                    if (!saveReplay.RequestStatus.Result)
                    {
                        Logger.Warning($"Failed to save replay buffer: {saveReplay.RequestStatus.Comment ?? "The replay buffer isn't running."}");
                    }
                }
            }
            catch
            {
                _webSocket.Dispose();
                _webSocket = null;
                throw;
            }
            finally
            {
                _busyLock.ReleaseMutex();
            }
        }

        private async Task<T> SendRequest<T>(Request<T> request) where T : RequestResponse
        {
            await SendMessage(request);
            return await ReceiveMessage(request.ResponseParser);
        }

        private async Task<RequestBatchResponse> SendRequestBatch(RequestBatch batch)
        {
            await SendMessage(batch);
            return await ReceiveMessage(RequestBatchResponse.Parse);
        }

        private async Task<T> ReceiveMessage<T>(Func<dynamic, T> extractMessage)
        {
            try
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(_receiveBuffer), CancellationToken.None);
                
                if (result.CloseStatus != null)
                {
                    // we need to complete the closing handshake, or OBS will log an improper closure.
                    try
                    {
                        await _webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                    catch { }

                    int closeCode = (int)result.CloseStatus;
                    if (CLOSE_CODE_AUTHENTICATION_FAILED == closeCode)
                    {
                        throw new ClientException("Failed to authenticate. Make sure that your password is correct.");
                    }

                    string description = result.CloseStatusDescription.Length == 0 ? "reason unknown" : result.CloseStatusDescription;
                    throw new ClientException($"Unexpected websocket closure: {description}");
                }
                
                var json = JSON.FromStream(new MemoryStream(_receiveBuffer, 0, result.Count));
                return extractMessage(json);
            }
            catch (ClientException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ClientException($"Unexpected error receiving a message ({e.GetType().Name}): {e.Message}", e);
            }
        }

        private async Task SendMessage<T>(T message) where T : Message
        {
            string json = message.ToJson();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            try
            {
                await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
            }
            catch (Exception e)
            {
                throw new ClientException($"Unexpected error sending a message ({e.GetType().Name}): {e.Message}", e);
            }
        }

        public async void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (_webSocket?.State == WebSocketState.Open)
            {
                try
                {
                    // we'll attempt to close, but there's nothing to do if it fails.
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
                catch { }
            }
            
            _busyLock.Dispose();
            _webSocket?.Dispose();
            _webSocket = null;
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
