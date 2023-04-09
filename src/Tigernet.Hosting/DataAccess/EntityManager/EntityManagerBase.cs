using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tigernet.Hosting.DataAccess.DataAccessContext;
using Tigernet.Hosting.DataAccess.Entity;
using Tigernet.Hosting.DataAccess.Query;
using Tigernet.Hosting.DataAccess.QueryOptions;

namespace Tigernet.Hosting.DataAccess.EntityManager;

/// <summary>
/// Provides base clever implementation
/// </summary>
/// <typeparam name="TEntity">Data type</typeparam>
/// <typeparam name="TKey">Data Id type</typeparam>
public class EntityManagerBase<TEntity> : IEntityManagerBase<TEntity> where TEntity : class, IEntity, IQueryableEntity
{
    private readonly IDataAccessContext _dataAccessContext;

    public EntityManagerBase(IDataAccessContext dataAccessContext)
    {
        _dataAccessContext = dataAccessContext;
    }

    public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression)
    {
        return _dataAccessContext.Set<TEntity>().Get(expression);
    }

    public virtual ValueTask<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        if (expression is null)
            throw new ArgumentException("Query expression cannot be null");

        return new ValueTask<IEnumerable<TEntity>>(_dataAccessContext.Set<TEntity>().Get(expression).ToList());
    }

    public virtual async ValueTask<IEnumerable<TEntity>> GetAsync(
        IEntityQueryOptions<TEntity> queryOptions,
        CancellationToken cancellationToken = default
    )
    {
        if (queryOptions is null)
            throw new ArgumentException("Query options cannot be null");

        return await _dataAccessContext.Set<TEntity>().Get(x => true).ApplyQuery(queryOptions).ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async ValueTask<TEntity?> GetFirstAsync(IEntityQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken = default)
    {
        if (queryOptions is null)
            throw new ArgumentException("Query options cannot be null");

        queryOptions.AddPagination(1, 1);
        return await _dataAccessContext.Set<TEntity>().Get(x => true).ApplyQuery(queryOptions).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async ValueTask<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dataAccessContext.Set<TEntity>().Get(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async ValueTask<TEntity> CreateAsync(TEntity entity, bool save = false, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentException("Cannot create null entity", nameof(entity));

        entity.Id = default;
        _dataAccessContext.Set<TEntity>().Add(entity);

        if (save)
            await _dataAccessContext.SaveAsync(cancellationToken);

        return entity;
    }

    public virtual async ValueTask<bool> UpdateAsync(TEntity entity, bool save = false, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentException("Cannot update null entity", nameof(entity));

        _dataAccessContext.Set<TEntity>().Update(entity);
        return save || await _dataAccessContext.SaveAsync(cancellationToken);
    }

    public virtual async ValueTask<bool> DeleteAsync(TEntity entity, bool save = false, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentException("Cannot delete null entity", nameof(entity));

        _dataAccessContext.Set<TEntity>().Delete(entity);
        return save || await _dataAccessContext.SaveAsync(cancellationToken);
    }
}