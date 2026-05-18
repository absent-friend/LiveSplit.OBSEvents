using System;
using System.Collections.Generic;

namespace LiveSplit.OBSEvents.OBS.Protocol
{
    internal abstract class Request<T> : Message where T : RequestResponse
    {
        private const int OPCODE = 6;

        protected override int OpCode => OPCODE;

        private string RequestId { get; } = Guid.NewGuid().ToString();

        protected virtual IDictionary<string, object> RequestData()
        {
            return null;
        }

        public abstract Func<dynamic, T> ResponseParser { get; }

        internal override IDictionary<string, object> FieldValues()
        {
            var values = new Dictionary<string, object>()
            {
                ["requestType"] = GetType().Name,
                ["requestId"] = RequestId,
            };

            IDictionary<string, object> requestData = RequestData();
            if (requestData != null)
            {
                values["requestData"] = requestData;
            }
            
            return values;
        }
    }
}
