using Tigernet.Hosting.Actions;
using Tigernet.Hosting.Attributes.HttpMethods;
using Tigernet.Hosting.Attributes.Resters;
using Tigernet.Hosting.Models.Query;
using Tigernet.Samples.RestApi.Clevers.Interfaces;
using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Resters
{
    [ApiRester]
    public class UsersRester : ResterBase
    {
        private readonly IUserClever userClever;

        public UsersRester(IUserClever userClever)
        {
            this.userClever = userClever;
        }

        [Poster("/by-filter")]
        public async ValueTask<object> GetByFilter(EntityQueryOptions<User> model)
        {
            return Ok(await userClever.GetAsync(model));
        }

        [Getter]
        public async ValueTask<object> Get()
        {
            var result = await userClever.GetByIdAsync(7);
            return Ok(result);
        }

        [Poster("/new")]
        public object Add()
        {
            User user = new User()
            {
                Id = 7,
                Name = "Sardor",
                Age = 28
            };

            return Ok(userClever.Add(user));
        }

        [Putter("/update")]
        public object Put()
        {
            User user = new User()
            {
                Id = 7,
                Name = "Ali",
                Age = 28
            };

            return Ok(userClever.Update(user.Id, user));
        }

        [Deleter("/delete")]
        public object Delete(int userUd)
        {
            var res = userClever.Delete(7);
            return Ok(res);
        }
    }
}