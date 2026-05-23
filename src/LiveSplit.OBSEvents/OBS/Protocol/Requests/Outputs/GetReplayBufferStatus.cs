using LiveSplit.OBSEvents.OBS.Protocol.Responses.Outputs;
using System;

namespace LiveSplit.OBSEvents.OBS.Protocol.Requests.Outputs
{
    internal class GetReplayBufferStatus : Request<GetReplayBufferStatusResponse>
    {
        public override Func<dynamic, GetReplayBufferStatusResponse> ResponseParser => GetReplayBufferStatusResponse.Parse;
    }
}
