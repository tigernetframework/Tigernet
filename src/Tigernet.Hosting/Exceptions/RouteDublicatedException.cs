namespace Tigernet.Hosting.Exceptions
{
    public class RouteDuplicatedException : Exception
    {
        public RouteDuplicatedException(string route)
            : base($"Route '{route}' already exists, please provide a unique route!")
        {
        }
    }
}