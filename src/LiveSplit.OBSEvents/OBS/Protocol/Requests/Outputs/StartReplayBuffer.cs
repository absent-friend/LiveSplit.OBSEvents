using LiveSplit.OBSEvents.OBS.Protocol.Responses.Outputs;
using System;

namespace LiveSplit.OBSEvents.OBS.Protocol.Requests.Outputs
{
    internal class StartReplayBuffer : Request<StartReplayBufferResponse>
    {
        public override Func<dynamic, StartReplayBufferResponse> ResponseParser => StartReplayBufferResponse.Parse;
    }
}
