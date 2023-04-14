using System.Runtime.Serialization;

namespace Tigernet.Hosting.DataAccess.Exceptions;

public class EntityCreateException : Exception
{
    public EntityCreateException()
    {
    }

    public EntityCreateException(string? message) : base(message)
    {
    }

    public EntityCreateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntityCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}