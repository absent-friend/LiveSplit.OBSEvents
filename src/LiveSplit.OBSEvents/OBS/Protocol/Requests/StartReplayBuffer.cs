using LiveSplit.OBSEvents.OBS.Protocol.Responses;
using System;

namespace LiveSplit.OBSEvents.OBS.Protocol.Requests
{
    internal class StartReplayBuffer : Request<StartReplayBufferResponse>
    {
        public override Func<dynamic, StartReplayBufferResponse> ResponseParser => StartReplayBufferResponse.Parse;
    }
}
