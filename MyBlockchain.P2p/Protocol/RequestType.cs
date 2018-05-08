using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlockchain.P2p.Protocol
{
    public enum RequestType
    {
        Ping,
        Store,
        FindNode,
        FindValue
    }
}
