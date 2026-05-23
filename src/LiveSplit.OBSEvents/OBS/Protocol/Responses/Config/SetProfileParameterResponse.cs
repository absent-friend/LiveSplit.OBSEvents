using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.OBSEvents.OBS.Protocol.Responses.Config
{
    internal class SetProfileParameterResponse(string id, RequestStatus status): RequestResponse(id, status)
    {
        public static SetProfileParameterResponse Parse(dynamic json)
        {
            dynamic data = ExtractAndValidateData(json);
            return Transform(data);
        }

        public static SetProfileParameterResponse Transform(dynamic data)
        {
            string id = data.requestId;
            RequestStatus status = RequestStatus.Parse(data.requestStatus);
            return new(id, status);
        }
    }
}
