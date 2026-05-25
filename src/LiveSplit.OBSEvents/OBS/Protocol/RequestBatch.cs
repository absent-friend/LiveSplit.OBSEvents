using System;
using System.Collections.Generic;
using System.Linq;

using LiveSplit.Web;

namespace LiveSplit.OBSEvents.OBS.Protocol
{
    internal class RequestBatch(IEnumerable<Message> requests) : Message
    {
        private const int OPCODE = 8;

        protected override int OpCode => OPCODE;

        private string RequestId { get; } = Guid.NewGuid().ToString();

        private IEnumerable<Message> Requests { get; } = requests;

        internal override IDictionary<string, object> FieldValues()
        {
            return new Dictionary<string, object>()
            {
                ["requestId"] = RequestId,
                ["haltOnFailure"] = true,
                ["requests"] = Requests.Select(req => new DynamicJsonObject(req.FieldValues()))
            };
        }
    }
}
