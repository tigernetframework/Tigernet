namespace Tigernet.Hosting.Exceptions;

public class NotImplementedException : Exception
{
    public NotImplementedException(string memberName)
        : base($"The member '{memberName}' has not been implemented, please check the documentation for more information.")
    {
    }

}
