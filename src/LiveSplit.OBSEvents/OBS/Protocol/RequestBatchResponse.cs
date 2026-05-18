using System.Collections.Generic;
using System.Linq;

namespace LiveSplit.OBSEvents.OBS.Protocol
{
    internal class RequestBatchResponse(string requestId, IEnumerable<dynamic> results)
    {
        private const int OPCODE = 9;

        public string RequestId { get; } = requestId;

        public IList<dynamic> Results { get; } = results.ToList();

        public static RequestBatchResponse Parse(dynamic json)
        {
            dynamic data = Message.ValidateAndExtractData(json, OPCODE, nameof(RequestBatchResponse));
            return new(data.requestId, (IEnumerable<dynamic>)data.results);
        }
    }
}
