using LiveSplit.OBSEvents.OBS.Protocol.Responses.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.OBSEvents.OBS.Protocol.Requests.Config
{
    internal class SetProfileParameter(string category, string name, string value) : Request<SetProfileParameterResponse>
    {
        public string Category { get; } = category;

        public string Name { get; } = name;

        public string Value { get; } = value;

        public override Func<dynamic, SetProfileParameterResponse> ResponseParser => SetProfileParameterResponse.Parse;

        protected override IDictionary<string, object> RequestData()
        {
            var data = new Dictionary<string, object>()
            {
                ["parameterCategory"] = Category,
                ["parameterName"] = Name
            };

            if (Value != null)
            {
                data["parameterValue"] = Value;
            }

            return data;
        }
    }
}
