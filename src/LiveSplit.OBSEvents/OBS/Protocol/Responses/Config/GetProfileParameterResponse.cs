namespace LiveSplit.OBSEvents.OBS.Protocol.Responses.Config
{
    internal class GetProfileParameterResponse(string id, RequestStatus status, string value): RequestResponse(id, status)
    {
        public string Value { get; } = value;

        public static GetProfileParameterResponse Parse(dynamic json)
        {
            dynamic data = ExtractAndValidateData(json);
            return Transform(data);
        }

        public static GetProfileParameterResponse Transform(dynamic data)
        {
            string id = data.requestId;
            RequestStatus status = RequestStatus.Parse(data.requestStatus);
            string value = status.Result ? data.responseData.parameterValue : null;
            return new(id, status, value);
        }
    }
}
