

namespace Tigernet.Hosting.Attributes.HttpMethods
{
    /// <summary>
    /// The 'DeleterAttribute' class used to represent a DELETE HTTP method.
    /// </summary>
    public class DeleterAttribute : HttpMethodAttribute
    {
        /// <inheritdoc />
        public DeleterAttribute(string route = null)
            : base(route)
        {
            
        }
        
        /// <inheritdoc />
        internal override string HttpMethodName
        {
            get => "DELETE";
        }
    }
}