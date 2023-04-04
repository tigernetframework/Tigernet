using Tigernet.Hosting.Attributes.Query;
using Tigernet.Hosting.Models.Common;

namespace Tigernet.Samples.RestApi.Models
{
    public class User : IEntity<int>, IQueryableEntity
    {
        public int Id { get; set; }
        
        [SearchableProperty]
        public string Name { get; set; }
        
        public int Age { get; set; }
    }
}