namespace LiveSplit.OBSEvents.OBS.Protocol
{
    internal abstract class RequestResponse(string requestId, RequestStatus requestStatus)
    {
        private const int OPCODE = 7;

        public string RequestId { get; } = requestId;

        public RequestStatus RequestStatus { get; } = requestStatus;

        protected static dynamic ExtractResponseData(dynamic json)
        {
            return Message.ValidateAndExtractData(json, OPCODE, nameof(RequestResponse));
        }
    }
}
