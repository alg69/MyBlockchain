using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlockchain.P2p.Exceptions
{

    [Serializable]
    public class InvalidNodeIdException : Exception
    {
        public InvalidNodeIdException() { }
        public InvalidNodeIdException(string message) : base(message) { }
        public InvalidNodeIdException(string message, Exception inner) : base(message, inner) { }
        protected InvalidNodeIdException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
