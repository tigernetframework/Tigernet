using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tigernet.Hosting.DataAccess.DataSourceProvider;
using Tigernet.Hosting.DataAccess.Exceptions;
using Tigernet.Hosting.DataAccess.Snapshot;

namespace Tigernet.Hosting.DataAccess.Entity;

public class EntitySet<TEntity> : IEntitySet<TEntity> where TEntity : class, IEntity
{
    private readonly IDataSourceProvider _dataSourceProvider;
    public List<IEntitySnapshot<TEntity>> EntitySnapshots { get; }

    public EntitySet(IDataSourceProvider dataSourceProvider)
    {
        _dataSourceProvider = dataSourceProvider;
        EntitySnapshots = new List<IEntitySnapshot<TEntity>>();
    }

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression)
    {
        // TODO : Implement get as queryable

        // Querying from data source
        var result = _dataSourceProvider.Query<TEntity>(expression.Compile());

        // Updating snapshots
        var updatedSnapshots = EntitySnapshots.Where(x => x.State != EntityState.Unchanged);
        var missingSnapshots = result.Where(x => !EntitySnapshots.Any(y => y.Snapshot.Id == x.Id));
        EntitySnapshots.AddRange(missingSnapshots.Select(x => new EntitySnapshot<TEntity>(x)));

        // Returning result
        return EntitySnapshots.Select(x => x.Entity).Where(expression.Compile()).AsQueryable();
    }

    public void Add(TEntity entity)
    {
        EntitySnapshots.Add(new EntitySnapshot<TEntity>(entity, EntityState.Added));
    }

    public void Update(TEntity entity)
    {
        var entitySnapshot = EntitySnapshots.FirstOrDefault(x => x.Entity?.Id == entity.Id) ?? throw new EntryUpdateException();
        entitySnapshot.State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        var entitySnapshot = EntitySnapshots.FirstOrDefault(x => x.Entity?.Id == entity.Id) ?? throw new EntryDeleteException();
        entitySnapshot.State = EntityState.Deleted;
    }
}