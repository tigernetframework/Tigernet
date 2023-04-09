using Tigernet.Hosting.DataAccess.Entity;

namespace Tigernet.Hosting.DataAccess.DataAccessContext;

public interface IDataAccessContext
{
    IEntitySet<TEntity> Set<TEntity>() where TEntity : class, IEntity;

    bool Save();

    ValueTask<bool> SaveAsync(CancellationToken cancellationToken = default);
}