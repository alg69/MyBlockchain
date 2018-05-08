using MyBlockchain.P2p.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.P2p.TestImpl
{
    public class Program
    {
        static void Main(string[] args)
        {
            var fakeIp = "127.0.0.1";
            var fakePort = 1337;
            var fakeNodeId = "22fa51f8acc80ced5748fb53f495df86398e4872";
            NetworkServer ns = new NetworkServer(fakeIp, fakePort, fakeNodeId);
            NetworkServer ns2 = new NetworkServer(fakeIp, fakePort+1);
            NetworkServer ns3 = new NetworkServer(fakeIp, fakePort+2);
            ns.SendRequest(new ContactInfo() { IpAddress = ns2.Node.IpAddress, Port = ns2.Node.Port, NodeId = ns2.Node.Id }, ns.Node.Id.NodeBytes);
            Console.ReadLine();
        }
    }
}
