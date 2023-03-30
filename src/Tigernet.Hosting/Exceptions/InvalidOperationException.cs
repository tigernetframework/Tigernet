namespace Tigernet.Hosting.Exceptions;

public class InvalidOperationException : Exception
{
    public InvalidOperationException()
        : base("Route is invalid, operation could not be performed!")
    {

    }
}