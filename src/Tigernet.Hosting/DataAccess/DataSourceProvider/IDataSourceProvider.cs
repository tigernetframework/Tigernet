using Tigernet.Hosting.DataAccess.Entity;

namespace Tigernet.Hosting.DataAccess.DataSourceProvider;

public interface IDataSourceProvider
{
    IQueryable<TEntity> Query<TEntity>(Func<TEntity, bool> expression) where TEntity : class, IEntity;
}