namespace LiveSplit.OBSEvents.OBS.Protocol.Responses
{
    internal class SaveReplayBufferResponse(string requestId, RequestStatus requestStatus) : RequestResponse(requestId, requestStatus)
    {
        public static SaveReplayBufferResponse Parse(dynamic json)
        {
            dynamic data = ExtractResponseData(json);
            return Transform(data);
        }

        public static SaveReplayBufferResponse Transform(dynamic data)
        {
            string id = data.requestId;
            RequestStatus status = RequestStatus.Parse(data.requestStatus);
            return new(id, status);
        }
    }
}
