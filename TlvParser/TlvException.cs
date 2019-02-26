using System;

namespace TlvParser
{
    public class TlvException : System.Exception
    {
        public TlvException() { }
        public TlvException(string message) : base(message) { }
        public TlvException(string message, System.Exception inner) : base(message, inner) { }
        protected TlvException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}