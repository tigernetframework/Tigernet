using Tigernet.Hosting.Attributes.Query;
using Tigernet.Hosting.DataAccess.Entity;

namespace Tigernet.Samples.RestApi.Models
{
    public class User : IEntity, IQueryableEntity
    {
        public long Id { get; set; }
        
        [SearchableProperty]
        public string Name { get; set; }
        
        public int Age { get; set; }
    }
}