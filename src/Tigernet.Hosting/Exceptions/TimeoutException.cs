namespace Tigernet.Hosting.Exceptions;

public class TimeoutException : Exception
{
    public TimeoutException(string message) : base(message) { }

    public TimeoutException(string message, Exception innerException)
        : base(message, innerException) { }

    public new string Message => "The operation has timed out.";
}
