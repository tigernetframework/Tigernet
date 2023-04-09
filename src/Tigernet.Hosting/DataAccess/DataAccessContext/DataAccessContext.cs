using Tigernet.Hosting.DataAccess.Entity;
using Tigernet.Hosting.DataAccess.Exceptions;

namespace Tigernet.Hosting.DataAccess.DataAccessContext;

public class DataAccessContext : IDataAccessContext
{
    private IDictionary<string, IEntitySet<IEntity>> EntitySets { get; set; }

    public DataAccessContext()
    {
        EntitySets = new Dictionary<string, IEntitySet<IEntity>>();
    }

    public IEntitySet<TEntity> Set<TEntity>() where TEntity : class, IEntity
    {
        return EntitySets.Select(x => x.Value).OfType<IEntitySet<TEntity>>().FirstOrDefault() ??
               throw new EntitySetNotRegisteredException(typeof(TEntity).Name);
    }

    public bool Save()
    {
        return true;
    }

    public ValueTask<bool> SaveAsync(CancellationToken cancellationToken = default)
    {
        return new ValueTask<bool>(true);
    }
}