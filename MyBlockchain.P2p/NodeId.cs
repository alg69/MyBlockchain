using MyBlockchain.P2p.Exceptions;
using MyBlockchain.P2p.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MyBlockchain.P2p
{
    /// <summary>
    /// Classe 'NodeId'
    /// </summary>
    public class NodeId : IEquatable<NodeId>, IComparable<NodeId>
    {
        /// <summary>
        /// Taille de l'identifant en bits.
        /// Dans Kademlia, celui-ci est fixé a 160 bits.
        /// </summary>
        public const uint DefaultNodeLength = 160;

        /// <summary>
        /// Le tableau de bytes de l'identifant.
        /// </summary>
        public byte[] NodeBytes { get; private set; }


        /// <summary>
        /// Instancie la classe avec un identifiant aléatoire.
        /// </summary>
        public NodeId()
        {
            Init(NodeId.GetRandomId());
        }

        /// <summary>
        /// Crée une instance de NodeId à l'aide de ses bytes.
        /// </summary>
        /// <param name="nodeBytes"></param>
        public NodeId(byte[] nodeBytes)
        {
            if (nodeBytes.Length * 8 != DefaultNodeLength)
                throw new InvalidNodeIdException("Node id does not contains the bytes length expected");

            NodeBytes = nodeBytes;
        }

        /// <summary>
        /// Crée une instance de NodeId à l'aide de son ID.
        /// </summary>
        /// <param name="hexNodeId"></param>
        public NodeId(string hexNodeId)
        {
            Init(hexNodeId);
        }

        /// <summary>
        /// Crée un identifiant aléatoire.
        /// </summary>
        /// <returns></returns>
        public static string GetRandomId()
        {
            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(RandomUtil.Instance.NextBytes());
            return HexUtil.ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        /// Initialise la classe.
        /// </summary>
        /// <param name="node"></param>
        private void Init(string id)
        {
            var nodeBytes = HexUtil.HexStringToByteArray(id);

            if (nodeBytes.Length * 8 != DefaultNodeLength)
                throw new InvalidNodeIdException("Node id does not contains the bytes length expected");

            NodeBytes = nodeBytes;
        }

        /// <summary>
        /// Conversion en string de l'identifiant du noeud.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return HexUtil.ByteArrayToHexString(NodeBytes);
        }

        /// <summary>
        /// Retourne la distance entre deux NodeId.
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        public static NodeId GetDistance(NodeId id1, NodeId id2)
        {
            uint len = NodeId.DefaultNodeLength / 8;
            var buf1 = id1.NodeBytes;
            var buf2 = id2.NodeBytes;

            if (buf1.Length != len || buf2.Length != len)
                throw new InvalidNodeIdException("Node id does not contains the bytes length expected");

            var bufXor = new byte[len];
            for (int i = 0; i < len; i++)
            {
                bufXor[i] = (byte)(buf1[i] ^ buf2[i]);
            }
            return new NodeId(bufXor);
        }

        /// <summary>
        /// Equatable: Les tableaux de bytes de l'identifiant doivent être identique.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(NodeId other)
        {
            return NodeBytes.SequenceEqual(other.NodeBytes);
        }

        /// <summary>
        /// Comparable: Règles pour comparer deux NodesId.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(NodeId other)
        {
            return Compare(this, other);
        }

        /// <summary>
        /// Règles pour comparer deux NodesId.
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns></returns>
        private static int Compare(NodeId n1, NodeId n2)
        {
            if (n1 is null && n2 is null)
                return 0;
            else if (n1 is null || n2 is null)
                return -1;

            var n1Int = BitConverter.ToUInt64(n1.NodeBytes.Reverse().ToArray(), 0);
            var n2Int = BitConverter.ToUInt64(n2.NodeBytes.Reverse().ToArray(), 0);

            if (n1Int > n2Int)
                return 1;
            else if (n1Int == n2Int)
                return 0;
            else
                return -1;
        }

        #region Operators
        public static bool operator !=(NodeId left, NodeId right)
        {
            return Compare(left, right) != 0;
        }

        public static bool operator ==(NodeId left, NodeId right)
        {
            return Compare(left, right) == 0;
        }

        public static bool operator <(NodeId left, NodeId right)
        {
            return Compare(left, right) < 0;
        }

        public static bool operator >(NodeId left, NodeId right)
        {
            return Compare(left, right) > 0;
        }

        public static bool operator <=(NodeId left, NodeId right)
        {
            return Compare(left, right) <= 0;
        }

        public static bool operator >=(NodeId left, NodeId right)
        {
            return Compare(left, right) >= 0;
        }

        public static NodeId operator ^(NodeId left, NodeId right)
        {
            return GetDistance(left, right);
        }
        #endregion
    }
}
