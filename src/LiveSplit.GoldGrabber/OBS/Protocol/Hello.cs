namespace LiveSplit.GoldGrabber.OBS.Protocol
{
    /// <summary>
    /// Message sent by OBS to begin the connection process and communicate features of the server.
    /// </summary>
    /// <param name="obsStudioVersion">The version of the OBS instance that we're connecting to.</param>
    /// <param name="obsWebSocketVersion">The version of obs-websocket that the OBS instance is running.</param>
    /// <param name="rpcVersion">The RPC version that OBS would like to use. We can select any version less than or equal to this value.</param>
    /// <param name="authentication">If present, contains an authentication challenge to which the client much respond.</param>
    internal class Hello(string obsStudioVersion, string obsWebSocketVersion, int rpcVersion, Authentication authentication)
    {
        const int OPCODE = 0;

        public string ObsStudioVersion { get; } = obsStudioVersion;

        public string ObsWebSocketVersion { get; } = obsWebSocketVersion;

        public int RpcVersion { get; } = rpcVersion;

        public Authentication Authentication { get; } = authentication;

        public static Hello Parse(dynamic json)
        {
            dynamic data = Message.ValidateAndExtractData(json, OPCODE, nameof(Hello));
            return new(data.obsStudioVersion, data.obsWebSocketVersion, data.rpcVersion, Authentication.OrNull(data.authentication));
        }
    }
}
