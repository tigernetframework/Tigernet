using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tigernet.Hosting.Attributes.Commons;

namespace Tigernet.Hosting.Attributes.HttpMethods
{
    public class PatcherAttribute : HttpMethodAttribute
    {
        public PatcherAttribute(string route = null)
        : base(route)
        {
        }

        /// <inheritdoc/>
        internal override string HttpMethodName { get => "Patch"; }
    }
}
