using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Abstractions
{
    public interface IUserClever
    {
        User Get(int id);
        IEnumerable<User> GetAll();
        User Add(User user);
    }
}
