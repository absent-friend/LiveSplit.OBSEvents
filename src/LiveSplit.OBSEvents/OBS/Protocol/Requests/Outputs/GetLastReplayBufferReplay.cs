using LiveSplit.OBSEvents.OBS.Protocol.Responses.Outputs;
using System;

namespace LiveSplit.OBSEvents.OBS.Protocol.Requests.Outputs
{
    internal class GetLastReplayBufferReplay : Request<GetLastReplayBufferReplayResponse>
    {
        public override Func<dynamic, GetLastReplayBufferReplayResponse> ResponseParser => GetLastReplayBufferReplayResponse.Parse;
    }
}
