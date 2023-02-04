using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tigernet.Hosting.Actions;
using Tigernet.Hosting.Attributes;

namespace Tigernet.Samples.RestApi.Resters
{
    public class HomeRester : ResterBase
    {
        [Getter]
        public object Index()
        {
            return Ok(new
            {
                Message = "Hello World!"
            });
        }
    }
}
