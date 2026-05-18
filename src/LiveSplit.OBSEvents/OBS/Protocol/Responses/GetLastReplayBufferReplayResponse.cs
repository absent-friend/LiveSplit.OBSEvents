namespace LiveSplit.OBSEvents.OBS.Protocol.Responses
{
    internal class GetLastReplayBufferReplayResponse(string requestId, RequestStatus requestStatus, string savedReplayPath) : RequestResponse(requestId, requestStatus)
    {
        public string SavedReplayPath { get; } = savedReplayPath;

        public static GetLastReplayBufferReplayResponse Parse(dynamic json)
        {
            dynamic data = ExtractResponseData(json);
            return Transform(data);
        }

        public static GetLastReplayBufferReplayResponse Transform(dynamic data)
        {
            string id = data.requestId;
            RequestStatus status = RequestStatus.Parse(data.requestStatus);
            string savedReplayPath = data.responseData.savedReplayPath;
            return new(id, status, savedReplayPath);
        }
    }
}
