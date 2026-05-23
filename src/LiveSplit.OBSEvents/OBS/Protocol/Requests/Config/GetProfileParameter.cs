using System;
using System.Collections.Generic;
using LiveSplit.OBSEvents.OBS.Protocol.Responses.Config;

namespace LiveSplit.OBSEvents.OBS.Protocol.Requests.Config
{
    internal class GetProfileParameter(string category, string name) : Request<GetProfileParameterResponse>
    {
        public string Category { get; } = category;

        public string Name { get; } = name;

        public override Func<dynamic, GetProfileParameterResponse> ResponseParser => GetProfileParameterResponse.Parse;

        protected override IDictionary<string, object> RequestData()
        {
            return new Dictionary<string, object>()
            {
                ["parameterCategory"] = Category,
                ["parameterName"] = Name
            };
        }
    }
}
