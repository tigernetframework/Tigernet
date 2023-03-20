namespace Tigernet.Hosting.Exceptions;

public class RouteUnavailableException : Exception
{
    public RouteUnavailableException(string routeName)
        : base($"The route '{routeName}' is unavailable,  please check the parameters and try again later.")
    {
    }
}