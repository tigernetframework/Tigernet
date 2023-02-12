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
