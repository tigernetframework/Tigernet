namespace Tigernet.Hosting.Exceptions;

public class RouteOperationFailedException : Exception
{
    public RouteOperationFailedException(string operation)
        : base($"The operation '{operation}' failed, please try again with valid input")
    {
    }
}