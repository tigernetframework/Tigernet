using Microsoft.EntityFrameworkCore;
using Tigernet.Hosting.DataAccess.Entity;

namespace Tigernet.Hosting.DataAccess.Snapshot;

public interface IEntitySnapshot<out TEntity> where TEntity : class, IEntity
{
    TEntity Snapshot { get;  }

    TEntity Entity { get; }

    EntityState State { set; get; }

    IEnumerable<string> ModifiedProperties { get; }
}