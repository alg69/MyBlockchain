using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyBlockchain.P2p.Utils
{
    /// <summary>
    /// Classe utilitaire de conversions Hex.
    /// </summary>
    public class HexUtil
    {
        /// <summary>
        /// Converti un string en tableau de bytes.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// Converti un tableau de bytes en string.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] array)
        {
            var sb = new StringBuilder();
            foreach (byte b in array)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
