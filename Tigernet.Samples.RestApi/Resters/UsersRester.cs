using Tigernet.Hosting.Actions;
using Tigernet.Hosting.Attributes.HttpMethods;
using Tigernet.Hosting.Attributes.RequestContents;
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
        public async ValueTask<object> GetByFilter([FromBody] EntityQueryOptions<User> model)
        {
            return Ok(await _userEntityManager.GetAsync(model));
        }

        [Getter]
        public async ValueTask<object> Get([FromBody]long id)
        {
            var result = await _userEntityManager.GetByIdAsync(id);
            return Ok(result);
        }

        [Poster("/new")]
        public async ValueTask<object> Add([FromBody]User user)
        {
            return Ok(await _userEntityManager.CreateAsync(user));
        }

        [Putter("/update")]
        public async ValueTask<object> Put([FromBody]User user)
        {
            return Ok(await _userEntityManager.UpdateAsync(user));
        }

        [Deleter("/delete")]
        public async ValueTask<object> Delete(long userId)
        {
            return Ok(await _userEntityManager.DeleteAsync(userId));
        }
    }
}