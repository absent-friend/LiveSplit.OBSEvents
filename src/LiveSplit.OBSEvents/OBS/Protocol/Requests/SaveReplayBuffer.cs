using System;

using LiveSplit.OBSEvents.OBS.Protocol.Responses;

namespace LiveSplit.OBSEvents.OBS.Protocol.Requests
{
    internal class SaveReplayBuffer : Request<SaveReplayBufferResponse>
    {
        public override Func<dynamic, SaveReplayBufferResponse> ResponseParser => SaveReplayBufferResponse.Parse;
    }
}
