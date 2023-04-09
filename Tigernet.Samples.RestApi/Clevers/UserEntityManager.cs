using Tigernet.Hosting.DataAccess.DataAccessContext;
using Tigernet.Hosting.DataAccess.EntityManager;
using Tigernet.Samples.RestApi.Clevers.Interfaces;
using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Clevers
{
    public class UserEntityManager : EntityManagerBase<User>, IUserEntityManager
    {
        private static List<User> users = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Mukhammadkarim",
                Age = 12
            },
            new User
            {
                Id = 2,
                Name = "Samandar",
                Age = 32
            },
            new User
            {
                Id = 3,
                Name = "Djakhongir",
                Age = 35
            },
            new User
            {
                Id = 4,
                Name = "Ixtiyor",
                Age = 56
            },
            new User
            {
                Id = 5,
                Name = "Yunusjon",
                Age = 34
            },
            new User
            {
                Id = 6,
                Name = "Sabohat",
                Age = 23
            },
        };

        public UserEntityManager() : base(new DataAccessContext())
        {
        }

        public User Add(User user)
        {
            users.Add(user);

            return user;
        }

        public User Update(User user)
        {
            var existedUser = users.FirstOrDefault(u => u.Id == user.Id);
            if (existedUser is null)
                return null;

            existedUser = user;
            existedUser.Id = user.Id;

            return existedUser;
        }
    }
}