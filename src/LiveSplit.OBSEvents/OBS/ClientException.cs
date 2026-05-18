using System;

namespace LiveSplit.OBSEvents.OBS
{
    internal class ClientException(string message, Exception inner) : Exception(message, inner)
    {
        public ClientException(string message): this(message, null) { }
    }
}
