using System.Runtime.Serialization;

namespace xSdk
{
    [Serializable]
    public class SdkException : Exception
    {
        public SdkException()
            : base() { }

        public SdkException(string message)
            : base(message) { }

        public SdkException(string message, Exception innerException)
            : base(message, innerException) { }

        protected SdkException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
