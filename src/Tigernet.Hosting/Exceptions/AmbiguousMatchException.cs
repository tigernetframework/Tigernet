namespace Tigernet.Hosting.Exceptions;

public class AmbiguousMatchException : Exception
{
    public AmbiguousMatchException()
        : base("An ambiguous match has been found. Please provide a more specific input to resolve the ambiguity.")
    {
    }
}
