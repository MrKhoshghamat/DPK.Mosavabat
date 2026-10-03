using System.Runtime.Serialization;

namespace DPK.Mosavabat.Common.Exceptions;

[Serializable]
public class DpkException : Exception
{
    public DpkException()
    {
    }

    public DpkException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }

    public DpkException(string message)
        : base(message)
    {
    }

    public DpkException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}