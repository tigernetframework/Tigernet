using Tigernet.Hosting.Attributes.HttpMethods.Commons;

namespace Tigernet.Hosting.Attributes.HttpMethods
{
    public class PatcherAttribute : HttpMethodAttribute
    {
        public PatcherAttribute(string route = null)
        : base(route)
        {
        }

        /// <inheritdoc/>
        internal override string HttpMethodName { get => "PATCH"; }
    }
}
