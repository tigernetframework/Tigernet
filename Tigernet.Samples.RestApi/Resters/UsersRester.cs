using Tigernet.Hosting.Actions;
using Tigernet.Hosting.Attributes.HttpMethods;
using Tigernet.Hosting.Attributes.Resters;
using Tigernet.Hosting.DataAccess.Models.Query;
using Tigernet.Hosting.DataAccess.Services;
using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Resters
{
    [ApiRester]
    public class UsersRester : ResterBase
    {
        private readonly IEntityManagerBaseService<User> _userEntityManager;

        public UsersRester(IEntityManagerBaseService<User> userEntityManager)
        {
            _userEntityManager = userEntityManager;
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
        public async ValueTask<object> Add()
        {
            User user = new User()
            {
                Id = 7,
                Name = "Ikrom",
                Age = 28
            };

            return Ok(await _userEntityManager.CreateAsync(user));
        }

        [Putter("/update")]
        public async ValueTask<object> Put()
        {
            User user = new User()
            {
                Id = 7,
                Name = "Ali",
                Age = 28
            };

            return Ok(await _userEntityManager.UpdateAsync(user));
        }
    }
}