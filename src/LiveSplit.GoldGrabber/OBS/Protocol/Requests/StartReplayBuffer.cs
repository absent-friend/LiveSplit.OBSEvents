using LiveSplit.GoldGrabber.OBS.Protocol.Responses;
using System;

namespace LiveSplit.GoldGrabber.OBS.Protocol.Requests
{
    internal class StartReplayBuffer : Request<StartReplayBufferResponse>
    {
        public override Func<dynamic, StartReplayBufferResponse> ResponseParser => StartReplayBufferResponse.Parse;
    }
}
