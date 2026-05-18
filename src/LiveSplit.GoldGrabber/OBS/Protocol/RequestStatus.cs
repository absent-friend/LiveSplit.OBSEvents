using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.GoldGrabber.OBS.Protocol
{
    internal class RequestStatus(bool result, int code, string comment)
    {
        public bool Result { get; } = result;

        public int Code { get; } = code;

        public string Comment { get; } = comment;

        public IDictionary<string, object> FieldValues()
        {
            var values = new Dictionary<string, object>
            {
                ["result"] = Result,
                ["code"] = Code
            };

            if (Comment != null)
            {
                values["comment"] = Comment;
            }

            return values;
        }

        public static RequestStatus Parse(dynamic data)
        {
            return new RequestStatus(data.result, data.code, data.comment);
        }
    }
}
