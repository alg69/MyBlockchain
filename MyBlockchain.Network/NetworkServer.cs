using MyBlockchain.P2p;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MyBlockchain.Network
{
    /// <summary>
    /// Serveur UDP avec Kademlia.
    /// </summary>
    public class NetworkServer
    {
        private Socket socket;
        private byte[] byteData = new byte[1024];
        private int port;
        private Node node;
 
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public NetworkServer(string ip, int port)
        {
            this.port = port;
            this.node = new Node(ip, port);
            this.Start();
        }

        /// <summary>
        /// Ctor avec NodeId.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="nodeId"></param>
        public NetworkServer(string ip, int port, string nodeId)
        {
            this.port = port;
            this.node = new Node(ip, port, nodeId);
            this.Start();
        }

        /// <summary>
        /// Start le serveur.
        /// </summary>
        public void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            WaitingNewClient();
            Debug.WriteLine($"New node created at {node.IpAddress}:{node.Port} with node id {node.Id}");
        }

        /// <summary>
        /// Stop le serveur.
        /// </summary>
        public void Stop()
        {
            socket.Close();
            Debug.WriteLine($"Local node closed");
        }

        /// <summary>
        /// Methode invokée à chaque nouvelle requête reçue par notre serveur UDP.
        /// </summary>
        /// <param name="iar"></param>
        private void ReceiveCallback(IAsyncResult iar)
        {
            Debug.WriteLine($"Receiving data from new node");
            EndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
            int dataLen = 0;
            byte[] data = null;
            try
            {
                dataLen = this.socket.EndReceiveFrom(iar, ref clientEP);
                data = new byte[dataLen];
                Array.Copy(this.byteData, data, dataLen);
                // Grab data here !
            }
            finally
            {
                WaitingNewClient();
            }
        }

        /// <summary>
        /// Attente d'une nouvelle connexion.
        /// </summary>
        private void WaitingNewClient()
        {
            EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
            this.socket.BeginReceiveFrom(this.byteData, 0, this.byteData.Length, SocketFlags.None, ref newClientEP, ReceiveCallback, newClientEP);
        }
    }
}
