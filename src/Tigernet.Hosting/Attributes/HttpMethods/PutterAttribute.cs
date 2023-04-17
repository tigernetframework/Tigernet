using Tigernet.Hosting.Attributes.HttpMethods.Commons;

namespace Tigernet.Hosting.Attributes.HttpMethods;
public class PutterAttribute : HttpMethodAttribute
{
    /// <inheritdoc />
    public PutterAttribute(string route = null)
        : base(route)
    {

    }

    /// <inheritdoc/>
    internal override string HttpMethodName { get => "PUT"; }
}