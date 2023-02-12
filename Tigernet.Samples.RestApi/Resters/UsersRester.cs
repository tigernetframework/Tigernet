using Tigernet.Hosting.Actions;
using Tigernet.Hosting.Attributes;
using Tigernet.Samples.RestApi.Abstractions;

namespace Tigernet.Samples.RestApi.Resters
{
    public class UsersRester : ResterBase
    {
        private readonly IUserClever userClever;

        public UsersRester(IUserClever userClever)
        {
            this.userClever = userClever;
        }

        [Getter(Route = "/all")]
        public object GetAll()
        {
            return Ok(userClever.GetAll());
        }

        [Getter]
        public object Get()
        {
            return Ok(userClever.Get(1));
        }
    }
}
