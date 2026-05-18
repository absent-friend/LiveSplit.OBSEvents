namespace LiveSplit.GoldGrabber.OBS.Protocol.Responses
{
    internal class StartReplayBufferResponse(string requestId, RequestStatus requestStatus) : RequestResponse(requestId, requestStatus)
    {
        public static StartReplayBufferResponse Parse(dynamic json)
        {
            dynamic data = ExtractResponseData(json);
            return Transform(data);
        }

        public static StartReplayBufferResponse Transform(dynamic data)
        {
            string id = data.requestId;
            RequestStatus status = RequestStatus.Parse(data.requestStatus);
            return new(id, status);
        }
    }
}
