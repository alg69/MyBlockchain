using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlockchain.P2p.Model
{
    /// <summary>
    /// Classe 'ContactInfo'
    /// Contient les informations d'une paire (Adresse IP, Port UDP, NodeId).
    /// </summary>
    public class ContactInfo
    {
        /// <summary>
        /// Adresse IP.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Port UDP.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Node Id.
        /// </summary>
        public NodeId NodeId { get; set; }
    }
}
