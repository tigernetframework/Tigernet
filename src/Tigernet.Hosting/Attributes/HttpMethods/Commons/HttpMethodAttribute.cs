namespace Tigernet.Hosting.Attributes.Commons;

/// <summary>
/// Identifies an action that supports a given set of HTTP methods.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public abstract class HttpMethodAttribute : Attribute 
{
    /// <summary>
    /// The <see href="route"/> field represents the route of the HTTP method decorated by the attribute.
    /// </summary>
    public readonly string route;

    /// <summary>
    /// Target http method value like "GET", "DELETE" 
    /// </summary>
    internal abstract string HttpMethodName { get; }

    /// <summary>
    /// The constructor of the class accepts a `route` parameter and sets the `route` field to its value.
    /// </summary>
    /// <param name="route">path of route</param>
    public HttpMethodAttribute(string route = null)
	{
		this.route = route;
	}
}
