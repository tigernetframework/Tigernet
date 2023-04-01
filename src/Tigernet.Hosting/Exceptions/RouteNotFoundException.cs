namespace Tigernet.Hosting.Exceptions;

public class RouteNotFoundException : Exception
{
    public RouteNotFoundException(string route, string message)
        : base($"The route '{route}' could not be found, please check the route details and provide a valid route!")
    {
    }
}