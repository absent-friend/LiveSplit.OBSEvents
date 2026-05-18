using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.GoldGrabber.OBS
{
    internal class ClientException(string message, Exception inner) : Exception(message, inner)
    {
        public ClientException(string message): this(message, null) { }
    }
}
