using System;
using System.Collections.Generic;

namespace LiveSplit.GoldGrabber.OBS.Protocol
{
    /// <summary>
    /// Message used to authenticate with the OBS websocket server and establish session parameters.
    /// </summary>
    /// <param name="rpcVersion">The highest RPC version that the client supports.</param>
    /// <param name="authentication">The answer to authentication challenge issued by OBS. Should be <c>null</c> if none was issued.</param>
    /// <param name="eventSubscriptions">A bitmask describing the events to which the client will subscribe. <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#eventsubscription"/></param>
    /// <seealso href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#identify-opcode-1"/>
    internal class Identify(int rpcVersion, string authentication, int? eventSubscriptions) : Message
    {
        const int OPCODE = 1;

        protected override int OpCode => OPCODE;

        public int RpcVersion { get; } = rpcVersion;

        // The response to the authentication challenge issued by OBS;
        // not to be confused with the object that contained the challenge...
        public string Authentication { get; } = authentication;

        public int? EventSubscriptions { get; } = eventSubscriptions;

        internal override IDictionary<string, object> FieldValues()
        {
            var values = new Dictionary<string, object>
            {
                ["rpcVersion"] = RpcVersion,
            };
            if (Authentication != null)
            {
                values["authentication"] = Authentication;
            }
            if (EventSubscriptions != null)
            {
                values["eventSubscriptions"] = EventSubscriptions;
            }
            return values;
        }
    }
}
