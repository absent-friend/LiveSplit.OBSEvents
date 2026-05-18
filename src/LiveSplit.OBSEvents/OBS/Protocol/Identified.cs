namespace LiveSplit.OBSEvents.OBS.Protocol
{
    internal class Identified(int negotiatedRpcVersion)
    {
        const int OPCODE = 2;

        public int NegotiatedRpcVersion { get; } = negotiatedRpcVersion;

        public static Identified Parse(dynamic json)
        {
            dynamic data = Message.ValidateAndExtractData(json, OPCODE, nameof(Identified));
            return new Identified(data.negotiatedRpcVersion);
        }
    }
}
