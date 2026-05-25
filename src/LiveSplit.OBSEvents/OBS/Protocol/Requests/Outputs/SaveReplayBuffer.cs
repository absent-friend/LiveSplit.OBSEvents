using System;
using LiveSplit.OBSEvents.OBS.Protocol.Responses.Outputs;

namespace LiveSplit.OBSEvents.OBS.Protocol.Requests.Outputs
{
    internal class SaveReplayBuffer : Request<SaveReplayBufferResponse>
    {
        public override Func<dynamic, SaveReplayBufferResponse> ResponseParser => SaveReplayBufferResponse.Parse;
    }
}
