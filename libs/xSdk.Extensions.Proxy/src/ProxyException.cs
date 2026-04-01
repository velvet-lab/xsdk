using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace xSdk;


[Serializable]
public class ProxyException : ApplicationException
{
    public ProxyException()
    {
    }

    public ProxyException(string? message) : base(message)
    {
    }

    public ProxyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ProxyException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
