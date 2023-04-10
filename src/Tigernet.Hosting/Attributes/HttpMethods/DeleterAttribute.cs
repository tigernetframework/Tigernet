using Tigernet.Hosting.Attributes.HttpMethods.Commons;
using Tigernet.Hosting.Exceptions;
using ArgumentNullException = Tigernet.Hosting.Exceptions.ArgumentNullException;

namespace Tigernet.Hosting.Attributes.HttpMethods
{
    /// <summary>
    /// The 'DeleterAttribute' class used to represent a DELETE HTTP method.
    /// </summary>
    public class DeleterAttribute : HttpMethodAttribute
    {
        public DeleterAttribute(string route = null)
            : base(route)
        {
            
        }
        
        internal override string HttpMethodName
        {
            get => "DELETE";
        }
    }
}