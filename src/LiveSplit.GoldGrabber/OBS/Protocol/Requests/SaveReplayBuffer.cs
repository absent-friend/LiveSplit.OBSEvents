using System;

using LiveSplit.GoldGrabber.OBS.Protocol.Responses;

namespace LiveSplit.GoldGrabber.OBS.Protocol.Requests
{
    internal class SaveReplayBuffer : Request<SaveReplayBufferResponse>
    {
        public override Func<dynamic, SaveReplayBufferResponse> ResponseParser => SaveReplayBufferResponse.Parse;
    }
}
