using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tigernet.Samples.RestApi.Models;

namespace Tigernet.Samples.RestApi.Abstractions
{
    public interface IUserClever
    {
        User Get(int id);
        IEnumerable<User> GetAll();
    }
}
