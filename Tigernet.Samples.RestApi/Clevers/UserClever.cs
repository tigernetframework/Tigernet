using Tigernet.Hosting.DataAccess.Clevers;
using Tigernet.Samples.RestApi.Clevers.Interfaces;
using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Clevers
{
    public class UserClever : CleverBase<User, int>, IUserClever
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
            }
        };

        public UserClever() : base(users.AsQueryable())
        {
        }

        public User Add(User user)
        {
            users.Add(user);

            return user;
        }
        

        public User Update(int userId, User user)
        {
            var existedUser = users.FirstOrDefault(u => u.Id == userId);
            if (existedUser is null)
                return null;

            existedUser = user;
            existedUser.Id = userId;

            return existedUser;
        }

        public bool Delete(int userId)
        {
            var userToDelete = users.FirstOrDefault(u => u.Id == userId);
            if (userToDelete != null)
            {
                users.Remove(userToDelete);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}