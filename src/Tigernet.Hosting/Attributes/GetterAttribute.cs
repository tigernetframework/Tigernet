using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigernet.Hosting.Attributes
{
    public class GetterAttribute : Attribute
    {
        public string Route { get; set; }
    }
}
