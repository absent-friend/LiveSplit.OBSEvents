using LiveSplit.GoldGrabber.OBS.Protocol.Responses;
using System;

namespace LiveSplit.GoldGrabber.OBS.Protocol.Requests
{
    internal class GetLastReplayBufferReplay : Request<GetLastReplayBufferReplayResponse>
    {
        public override Func<dynamic, GetLastReplayBufferReplayResponse> ResponseParser => GetLastReplayBufferReplayResponse.Parse;
    }
}
