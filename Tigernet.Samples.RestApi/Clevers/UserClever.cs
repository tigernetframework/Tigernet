using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tigernet.Samples.RestApi.Abstractions;
using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Clevers
{
    public class UserClever : IUserClever
    {
        public User Get(int id)
        {
            return GetAll().FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return new List<User>
            {
                new User { Id = 1, Name = "Mukhammadkarim", Age = 12 },
                new User { Id = 2, Name = "Samandar", Age = 32 },
                new User { Id = 3, Name = "Djakhongir", Age = 35 },
                new User { Id = 4, Name = "Ixtiyor", Age = 56 },
                new User { Id = 5, Name = "Yunusjon", Age = 34 },
                new User { Id = 6, Name = "Sabohat", Age = 23 },
            };
        }
    }
}
