using System.Runtime.Serialization;

namespace Tigernet.Hosting.DataAccess.Exceptions;

public class EntryDeleteException : Exception
{
    public EntryDeleteException()
    {
    }

    public EntryDeleteException(string? message) : base(message)
    {
    }

    public EntryDeleteException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntryDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}