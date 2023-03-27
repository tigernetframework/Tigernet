namespace Tigernet.Hosting.Exceptions;

public class ArgumentOutOfRangeException : Exception
{
    public ArgumentOutOfRangeException(string argumentName, object actualValue)
        : base($"The argument '{argumentName}' with value '{actualValue}' is out of range, please provide a valid input!")
    {
    }
}