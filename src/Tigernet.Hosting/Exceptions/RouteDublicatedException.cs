using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigernet.Hosting.Exceptions
{
    public class RouteDublicatedException : Exception
    {
        public override string Message => "Route was dublicated, you have already added this route!";
    }
}
