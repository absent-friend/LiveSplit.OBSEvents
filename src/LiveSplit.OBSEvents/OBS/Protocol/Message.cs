using System;
using System.Collections.Generic;
using LiveSplit.Web;

namespace LiveSplit.OBSEvents.OBS.Protocol
{
    internal abstract class Message
    {
        protected abstract int OpCode { get; }

        internal abstract IDictionary<string, object> FieldValues();

        public string ToJson()
        {
            var values = new Dictionary<string, object>()
            {
                ["op"] = OpCode,
                ["d"] = FieldValues()
            };
            return new DynamicJsonObject(values).ToString();
        }

        internal static dynamic ValidateAndExtractData(dynamic json, int expectedOpCode, string expectedType)
        {
            int? op = json.op;
            dynamic data = json.d;

            if (op == null || data == null)
            {
                throw new Exception($"Invalid message from OBS websocket server. message: {json}");
            }
            else if (expectedOpCode != op)
            {
                throw new Exception($"Unexpected opcode {op} for {expectedType} message. data: {data}");
            }

            return data;
        }
    }
}
