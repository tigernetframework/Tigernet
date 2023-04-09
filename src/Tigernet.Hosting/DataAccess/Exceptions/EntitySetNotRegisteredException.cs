using System.Runtime.Serialization;

namespace Tigernet.Hosting.DataAccess.Exceptions;

public class EntitySetNotRegisteredException : Exception
{
    public EntitySetNotRegisteredException()
    {
    }

    public EntitySetNotRegisteredException(string? message) : base(message)
    {
    }

    public EntitySetNotRegisteredException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntitySetNotRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}