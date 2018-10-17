using System;
using System.Runtime.Serialization;

namespace LowLevelKeyboardLib.Input
{
    [Serializable]
    internal class AlreadyInterceptedException : Exception
    {
        public AlreadyInterceptedException()
        {
        }

        public AlreadyInterceptedException(string message) : base(message)
        {
        }

        public AlreadyInterceptedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlreadyInterceptedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}