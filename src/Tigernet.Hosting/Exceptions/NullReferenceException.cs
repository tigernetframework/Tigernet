namespace Tigernet.Hosting.Exceptions;

public class NullReferenceException : Exception
{
    public NullReferenceException()
        : base("A null reference was encountered. Please make sure that all objects are initialized before using them.")
    {
    }
}