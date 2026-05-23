using LiveSplit.Model;
using LiveSplit.OBSEvents.OBS.Protocol;
using LiveSplit.OBSEvents.OBS.Protocol.Requests.Config;
using LiveSplit.OBSEvents.OBS.Protocol.Requests.Outputs;
using LiveSplit.OBSEvents.OBS.Protocol.Responses.Config;
using LiveSplit.OBSEvents.OBS.Protocol.Responses.Outputs;
using LiveSplit.OBSEvents.UI;
using LiveSplit.OBSEvents.Utility;
using LiveSplit.Web;
using System;
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

        public async Task SaveBestSegmentReplay(LiveSplitState state, int splitIndex, TimeSpan segmentTime)
        {
            if (_webSocket == null || !_busyLock.WaitOne(TimeSpan.Zero))
            {
                return;
            }
            
            try
            {
                // first batch: grab initial state of replay buffer settings
                RequestBatchResponse paramValues = await SendRequestBatch(
                    new GetReplayBufferStatus(),
                    new GetProfileParameter("Output", "Mode"),
                    new GetProfileParameter("Output", "FilenameFormatting"),
                    new GetProfileParameter("SimpleOutput", "RecRBPrefix"),
                    new GetProfileParameter("SimpleOutput", "RecRBSuffix"),
                    new GetProfileParameter("AdvOut", "RecRBPrefix"),
                    new GetProfileParameter("AdvOut", "RecRBSuffix"));

                GetReplayBufferStatusResponse replayStatus = GetReplayBufferStatusResponse.Transform(paramValues.Results[0]);
                if (!replayStatus.IsActive.HasValue || !replayStatus.IsActive.Value)
                {
                    Logger.Error("Can't save replay buffer; make that it is enabled and running.");
                    return;
                }

                GetProfileParameterResponse mode = GetProfileParameterResponse.Transform(paramValues.Results[1]);
                GetProfileParameterResponse filenameFormatting = GetProfileParameterResponse.Transform(paramValues.Results[2]);
                int prefixIndex = mode.Value == "Simple" ? 3 : 5;
                GetProfileParameterResponse prefix = GetProfileParameterResponse.Transform(paramValues.Results[prefixIndex]);
                int suffixIndex = mode.Value == "Simple" ? 4 : 6;
                GetProfileParameterResponse suffix = GetProfileParameterResponse.Transform(paramValues.Results[suffixIndex]);

                string newReplayName = ReplayFilenameFormatter.Format(_settings.ReplayNameFormat, state, splitIndex, segmentTime);
                string categoryForMode = mode.Value == "Simple" ? "SimpleOutput" : "AdvOut";
                // second batch: set the replay filename temporarily and save the replay
                RequestBatchResponse replaySave = await SendRequestBatch(
                    new SetProfileParameter("Output", "FilenameFormatting", newReplayName),
                    new SetProfileParameter(categoryForMode, "RecRBPrefix", ""),
                    new SetProfileParameter(categoryForMode, "RecRBSuffix", ""),
                    new SaveReplayBuffer());

                SaveReplayBufferResponse saveResult = SaveReplayBufferResponse.Transform(replaySave.Results[3]);
                if (!saveResult.RequestStatus.Result)
                {
                    Logger.Error($"Failed to save replay buffer: {saveResult.RequestStatus.Comment ?? "unknown error"}");
                }

                // third batch: restore any changed settings to their original values
                await SendRequestBatch(
                    new SetProfileParameter("Output", "FilenameFormatting", filenameFormatting.Value),
                    new SetProfileParameter(categoryForMode, "RecRBPrefix", prefix.Value),
                    new SetProfileParameter(categoryForMode, "RecRBSuffix", suffix.Value));
            }
            catch (Exception e)
            {
                Logger.Error($"Unexpected error saving replay. Please file an issue on GitHub. stacktrace:\r\n{e.StackTrace}");
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

        private async Task<RequestBatchResponse> SendRequestBatch(params Message[] requests)
        {
            var batch = new RequestBatch(requests);
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
