using LiveSplit.Model;
using LiveSplit.OBSEvents.OBS.Protocol;
using LiveSplit.OBSEvents.OBS.Protocol.Requests.Outputs;
using LiveSplit.OBSEvents.OBS.Protocol.Responses.Outputs;
using LiveSplit.OBSEvents.UI;
using LiveSplit.OBSEvents.Utility;
using LiveSplit.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiveSplit.OBSEvents.OBS
{
    internal class Client : IDisposable
    {
        private const int CLOSE_CODE_AUTHENTICATION_FAILED = 4009;
        private const int REQUEST_STATUS_OUTPUT_RUNNING = 500;
        private const int EVENT_SUBSCRIPTIONS_NONE = 0;
        
        private readonly Mutex _busyLock = new();
        private readonly Uri _uri;
        private readonly string _password;
        private readonly OBSEventsSettings _settings;
        private readonly byte[] _receiveBuffer = new byte[4096];
        private ClientWebSocket _webSocket;
        private bool _disposed;

        internal Client(string host, int port, string password, OBSEventsSettings settings)
        {
            _uri = new Uri($"ws://{host}:{port}");
            _password = password;
            _settings = settings;
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
                await RunStartupTasks();
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

        private async Task RunStartupTasks()
        {
            IList<Message> requests = [];
            if (_settings.SaveBestSegmentReplay)
            {
                requests.Add(new StartReplayBuffer());
            }

            RequestBatchResponse taskResults = await SendRequestBatch(new RequestBatch(requests));
            int i = 0;
            if (_settings.SaveBestSegmentReplay)
            {
                StartReplayBufferResponse startBuffer = StartReplayBufferResponse.Transform(taskResults.Results[i++]);
                if (!startBuffer.RequestStatus.Result && startBuffer.RequestStatus.Code != REQUEST_STATUS_OUTPUT_RUNNING)
                {
                    Logger.Error($"Failed to start the replay buffer: {startBuffer.RequestStatus.Comment}");
                    return;
                }
            }
        }

        public async Task SaveBestSegmentReplay(LiveSplitState state, int splitIndex, TimeSpan segmentTime)
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
                        
                        while (!File.Exists(lastReplay.SavedReplayPath))
                        {
                            // The first saved replay takes extra time to show up for some reason.
                            // Timestamps might not line up either. Need to keep asking until we get a match
                            await Task.Delay(TimeSpan.FromSeconds(1));
                            lastReplay = await SendRequest(new GetLastReplayBufferReplay());
                        }
                        
                        string newReplayName = ReplayFilenameFormatter.Format(_settings.ReplayNameFormat, state, splitIndex, segmentTime);
                        try
                        {
                            FileOperations.Rename(lastReplay.SavedReplayPath, newReplayName);
                        }
                        catch (Exception ex)
                        {
                            Logger.Warning($"Failed to rename replay file: {ex.Message}");
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
