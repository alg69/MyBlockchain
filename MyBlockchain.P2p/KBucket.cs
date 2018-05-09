using MyBlockchain.P2p.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyBlockchain.P2p
{
    /// <summary>
    /// KBucket
    /// </summary>
    public class KBucket : List<ContactInfo>
    {
        /// <summary>
        /// Taille maximum du bucket
        /// </summary>
        public uint MaxBucketSize { get; }

        /// <summary>
        /// Est-ce que le bucket est plein ? 
        /// </summary>
        /// <returns></returns>
        public bool IsFull() => this.Count == MaxBucketSize;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="maxBucketSize"></param> 
        public KBucket(uint maxBucketSize) :
            base((int)maxBucketSize)
        {
            MaxBucketSize = maxBucketSize;
        }

        /// <summary>
        /// Récupère les infos du contact en fonction de son node Id.
        /// Renvoi null si l'objet n'existe pas dans le bucket.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContactInfo GetContactInfo(NodeId id)
        {
            return this.FirstOrDefault(x => x.NodeId == id);
        }

        /// <summary>
        /// Déplace le peerinfo à la fin du K-Bucket.
        /// </summary>
        /// <param name="id"></param>
        public void MoveToEnd(NodeId id)
        {
            if (!IsNodeIdExists(id)) return;
            var contactInfo = this.First(x => x.NodeId == id);
            this.Remove(contactInfo);
            this.Add(contactInfo);
        }

        /// <summary>
        /// Déplace le peerinfo au début du K-Bucket.
        /// </summary>
        /// <param name="id"></param>
        public void MoveToHead(NodeId id)
        {
            if (!IsNodeIdExists(id)) return;
            this.Insert(0, this.First(x => x.NodeId == id));
        }

        /// <summary>
        /// Est-ce que le noeud existe dans le K-Bucket ?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsNodeIdExists(NodeId id)
        {
            return Exists(x => x.NodeId == id);
        }
    }
}
