using System;
using System.Runtime.Serialization;

namespace InterceptKeyboardLib.Input
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