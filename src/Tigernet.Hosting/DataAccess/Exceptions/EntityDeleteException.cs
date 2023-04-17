using System.Runtime.Serialization;

namespace Tigernet.Hosting.DataAccess.Exceptions;

public class EntityDeleteException : Exception
{
    public EntityDeleteException()
    {
    }

    public EntityDeleteException(string? message) : base(message)
    {
    }

    public EntityDeleteException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntityDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}