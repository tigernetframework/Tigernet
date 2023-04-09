using System.Runtime.Serialization;

namespace Tigernet.Hosting.DataAccess.Exceptions;

public class EntryUpdateException : Exception
{
    public EntryUpdateException()
    {
    }

    public EntryUpdateException(string? message) : base(message)
    {
    }

    public EntryUpdateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EntryUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}