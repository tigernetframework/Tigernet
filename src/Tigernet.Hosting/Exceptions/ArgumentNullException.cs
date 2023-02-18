namespace Tigernet.Hosting.Exceptions;

public class ArgumentNullException : Exception
{
    public ArgumentNullException(string argumentName)
        :base( $"The argument '{argumentName}' cannot be null, please provide a valid input!")
    {
    }
    
}
