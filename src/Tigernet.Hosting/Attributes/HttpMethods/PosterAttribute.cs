using Tigernet.Hosting.Attributes.HttpMethods.Commons;

namespace Tigernet.Hosting.Attributes.HttpMethods;

/// <summary>
/// The `PosterAttribute` class is used to represent a POST HTTP method.
/// </summary>
public class PosterAttribute : HttpMethodAttribute
{
    /// <inheritdoc />
	public PosterAttribute(string route = null)
        : base(route)
    {

    }

    /// <inheritdoc/>
    internal override string HttpMethodName { get => "POST"; }

}
