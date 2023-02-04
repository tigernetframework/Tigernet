using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tigernet.Hosting.Actions
{
    public abstract class ResterBase
    {
        public object Ok(object data)
        {
            return JsonSerializer.Serialize(data);
        }
    }
}
