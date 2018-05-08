using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MyBlockchain.P2p.Utils
{
    /// <summary>
    /// Singleton pour récupérer les données aléatoires.
    /// </summary>
    public class RandomUtil
    {
        /// <summary>
        /// Instance du singleton.
        /// </summary>
        private static readonly RandomUtil instance = new RandomUtil();
        public static RandomUtil Instance => instance;

        /// <summary>
        /// Crypto provider.
        /// </summary>
        private readonly RNGCryptoServiceProvider provider;

        /// <summary>
        /// Ctor.
        /// </summary>
        RandomUtil()
        {
            provider = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Génère un tableau de bytes contenant des bytes aléatoires.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] NextBytes(uint value = 128)
        {
            byte[] buffer = new byte[value];
            provider.GetBytes(buffer);
            return buffer;
        }
    }
}
