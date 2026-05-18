using LiveSplit.OBSEvents.OBS.Protocol.Responses;
using System;

namespace LiveSplit.OBSEvents.OBS.Protocol.Requests
{
    internal class GetLastReplayBufferReplay : Request<GetLastReplayBufferReplayResponse>
    {
        public override Func<dynamic, GetLastReplayBufferReplayResponse> ResponseParser => GetLastReplayBufferReplayResponse.Parse;
    }
}
