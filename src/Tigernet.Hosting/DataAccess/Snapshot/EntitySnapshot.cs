using Microsoft.EntityFrameworkCore;
using Tigernet.Hosting.DataAccess.Entity;

namespace Tigernet.Hosting.DataAccess.Snapshot;

public class EntitySnapshot<TEntity> : IEntitySnapshot<TEntity> where TEntity : class, IEntity
{
    public TEntity Entity { get; }

    public TEntity Snapshot { get; set; }

    public EntityState State { get; set; }

    public IEnumerable<string> ModifiedProperties { get; }

    public EntitySnapshot(TEntity entity)
    {
        Snapshot = entity;
        Entity = entity;
        State = EntityState.Unchanged;
        ModifiedProperties = new List<string>();
    }

    public EntitySnapshot(TEntity entity, EntityState state)
    {
        Snapshot = entity;
        Entity = entity;
        State = state;
        ModifiedProperties = new List<string>();
    }
}