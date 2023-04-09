using Tigernet.Hosting.DataAccess.EntityManager;
using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Clevers.Interfaces;

public interface IUserEntityManager : IEntityManagerBase<User>
{
    User Add(User user);

    User Update(User user);
}