using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tigernet.Hosting.Models.Common
{
    public class Auditable<TKey>
    {
        public TKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}