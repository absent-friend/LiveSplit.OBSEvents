namespace LiveSplit.OBSEvents.OBS.Protocol.Responses.Outputs
{
    internal class GetReplayBufferStatusResponse(string id, RequestStatus status, bool? active): RequestResponse(id, status)
    {
        public bool? IsActive { get; } = active;

        public static GetReplayBufferStatusResponse Parse(dynamic json)
        {
            dynamic data = ExtractAndValidateData(json);
            return Transform(data);
        }

        public static GetReplayBufferStatusResponse Transform(dynamic data)
        {
            string id = data.requestId;
            RequestStatus status = RequestStatus.Parse(data.requestStatus);
            bool? active = data.responseData?.outputActive;
            return new(id, status, active);
        }
    }
}
