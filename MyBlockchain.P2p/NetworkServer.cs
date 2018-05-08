using MyBlockchain.P2p.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.P2p
{
    /// <summary>
    /// Serveur UDP avec Kademlia.
    /// </summary>
    public class NetworkServer
    {
        /// <summary>
        /// Socket UDP.
        /// </summary>
        private Socket socket;
        private Socket clientSocket;
        /// <summary>
        /// Buffer.
        /// </summary>
        private byte[] buffer = new byte[1024];

        /// <summary>
        /// Port d'écoute.
        /// </summary>
        private int port;

        /// <summary>
        /// Noeud Kademlia.
        /// </summary>
        private Node node;
        public Node Node => node;

        /// <summary>
        /// Timeout
        /// </summary>
        private const int SendTimeout = 300;
 
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
                dataLen = socket.EndReceiveFrom(iar, ref clientEP);
                data = new byte[dataLen];
                Array.Copy(buffer, data, dataLen);
                HandleRequest(data);
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
            socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref newClientEP, ReceiveCallback, newClientEP);
        }

        public void SendRequest(ContactInfo info, byte[] data)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            {
                SendTimeout = SendTimeout
            };
            var socketEventArgs = new SocketAsyncEventArgs
            {
                RemoteEndPoint = new IPEndPoint(IPAddress.Parse(info.IpAddress), info.Port)
            };
            var remoteEP = new IPEndPoint(IPAddress.Parse(info.IpAddress), info.Port);
            clientSocket.BeginConnect(remoteEP, ConnectionCallback, data);
        }

        private void ConnectionCallback(IAsyncResult ar)
        {
            var data = (byte[])ar.AsyncState;

            if (!clientSocket.Connected) return;
            clientSocket.Send(data);
        }

        /// <summary>
        /// Traitement de la requête.
        /// </summary>
        /// <param name="data"></param>
        private void HandleRequest(byte[] data)
        {
            Debug.WriteLine($"New node discovered with id {new NodeId(data)}");
        }
    }
}
