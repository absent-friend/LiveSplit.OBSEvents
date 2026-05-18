using System.Collections.Generic;

namespace LiveSplit.OBSEvents.OBS.Protocol
{
    internal class Authentication
    {
        private Authentication(string challenge, string salt)
        {
            Challenge = challenge;
            Salt = salt;
        }

        public string Challenge { get; }

        public string Salt { get; }

        public IDictionary<string, object> FieldValues()
        {
            return new Dictionary<string, object>
            {
                ["challenge"] = Challenge,
                ["salt"] = Salt
            };
        }

        public static Authentication OrNull(dynamic json)
        {
            if (json == null)
            {
                return null;
            }
            return new Authentication(json.challenge, json.salt);
        }
    }
}
