namespace Tigernet.Hosting.Exceptions
{
    public class RouteDublicatedException : Exception
    {
        public override string Message => "Route was dublicated, you have already added this route!";
    }
}
