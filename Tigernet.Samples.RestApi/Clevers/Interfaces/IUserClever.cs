using Tigernet.Hosting.DataAccess.Clevers;
using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Clevers.Interfaces;

public interface IUserClever : ICleverBase<User, int>
{
    User Add(User user);
    User Update(int userId, User user);
}