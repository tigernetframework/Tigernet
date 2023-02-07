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

        [Getter("/show")]
        public object ShowMessage()
        {
            return Ok(new
            {
                Message = "Check for dublication of routes"
            });
        }
    }
}
