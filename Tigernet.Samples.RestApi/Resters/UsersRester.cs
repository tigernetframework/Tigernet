using Tigernet.Hosting.Actions;
using Tigernet.Hosting.Attributes.HttpMethods;
using Tigernet.Hosting.Attributes.Resters;
using Tigernet.Hosting.DataAccess.Query;
using Tigernet.Samples.RestApi.Clevers.Interfaces;
using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Resters
{
    [ApiRester]
    public class UsersRester : ResterBase
    {
        private readonly IUserEntityManager _userEntityManager;

        public UsersRester(IUserEntityManager userEntityManager)
        {
            this._userEntityManager = userEntityManager;
        }

        [Poster("/by-filter")]
        public async ValueTask<object> GetByFilter(EntityQueryOptions<User> model)
        {
            return Ok(await _userEntityManager.GetAsync(model));
        }

        [Getter]
        public async ValueTask<object> Get()
        {
            var result = await _userEntityManager.GetByIdAsync(1);
            return Ok(result);
        }

        [Poster("/new")]
        public object Add()
        {
            User user = new User()
            {
                Id = 7,
                Name = "Ikrom",
                Age = 28
            };

            return Ok(_userEntityManager.Add(user));
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
            
            return Ok(_userEntityManager.Update(user));
        }
    }
}