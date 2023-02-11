using Tigernet.Hosting.Attributes.Commons;

namespace Tigernet.Hosting.Attributes.HttpMethods;

/// <summary>
/// The `GetterAttribute` class is used to represent a GET HTTP method.
/// </summary>
public class GetterAttribute : HttpMethodAttribute
{
	/// <inheritdoc />
	public GetterAttribute(string route = null)
		: base(route)
	{
	}
    
	/// <inheritdoc/>
    internal override string HttpMethodName { get => "GET"; }
}
