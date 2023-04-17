using System.Runtime.Serialization;

namespace Tigernet.Hosting.DataAccess.Exceptions;

public class EntityUpdateException : Exception
{
    public EntityUpdateException()
    {
    }

    public EntityUpdateException(string? message) : base(message)
    {
    }

    public EntityUpdateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntityUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}