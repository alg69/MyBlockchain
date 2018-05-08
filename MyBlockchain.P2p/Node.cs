using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlockchain.P2p
{
    /// <summary>
    /// Noeud Kademlia.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Le paramètre k du whitepaper Kademlia.
        /// </summary>
        public const uint BucketsSize = 20;

        /// <summary>
        /// Id du noeud.
        /// </summary>
        public NodeId Id { get; }

        /// <summary>
        /// Liste de K-Buckets.
        /// </summary>
        public List<KBucket> Buckets { get; private set; }

        /// <summary>
        /// Adresse IP du noeud.
        /// </summary>
        public string IpAddress { get; private set; }

        /// <summary>
        /// Port UDP.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Construit l'objet Node avec son node ID.
        /// </summary>
        /// <param name="id"></param>
        public Node(string ip, int port, string id)
        {
            Id = new NodeId(id);
            SetUpInfos(ip, port);
            InitBuckets();
        }

        /// <summary>
        /// Construit l'objet Node avec un node ID aléatoire.
        /// </summary>
        public Node(string ip, int port)
        {
            Id = new NodeId();
            SetUpInfos(ip, port);
            InitBuckets();
        }

        /// <summary>
        /// Set l'adresse IP et le port dans le noeud.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private void SetUpInfos(string ip, int port)
        {
            IpAddress = ip;
            Port = port;
        }

        /// <summary>
        /// Initialisation des K-Buckets.
        /// </summary>
        private void InitBuckets()
        {
            Buckets = new List<KBucket>();
            for (int i = 0; i < NodeId.DefaultNodeLength; i++)
            {
                Buckets.Add(new KBucket(BucketsSize));
            }
        }
    }
}
