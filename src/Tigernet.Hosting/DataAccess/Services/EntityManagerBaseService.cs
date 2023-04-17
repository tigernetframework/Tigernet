using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tigernet.Hosting.DataAccess.Brokers;
using Tigernet.Hosting.DataAccess.Exceptions;
using Tigernet.Hosting.DataAccess.Models.Entity;
using Tigernet.Hosting.DataAccess.Models.Query;
using Tigernet.Hosting.DataAccess.Models.QueryOptions;

namespace Tigernet.Hosting.DataAccess.Services;

/// <summary>
/// Provides base clever implementation
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public class EntityManagerBaseService<TEntity> : IEntityManagerBaseService<TEntity> where TEntity : class, IEntity, IQueryableEntity
{
    private readonly IDataSourceBroker _dataSourceBroker;

    public EntityManagerBaseService(IDataSourceBroker dataSourceBroker)
    {
        _dataSourceBroker = dataSourceBroker;
    }

    public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression)
    {
        return _dataSourceBroker.Get(expression);
    }

    public virtual ValueTask<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        if (expression is null)
            throw new ArgumentException("Query expression cannot be null");

        return new ValueTask<IEnumerable<TEntity>>(_dataSourceBroker.Get(expression).ToList());
    }

    public virtual async ValueTask<IEnumerable<TEntity>> GetAsync(
        IEntityQueryOptions<TEntity> queryOptions,
        CancellationToken cancellationToken = default
    )
    {
        if (queryOptions is null)
            throw new ArgumentException("Query options cannot be null");

        Expression<Func<TEntity, bool>> initialPredicate = x => true;
        return await _dataSourceBroker.Get<TEntity>(x => true).ApplyQuery(queryOptions).ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async ValueTask<TEntity?> GetFirstAsync(IEntityQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken = default)
    {
        if (queryOptions is null)
            throw new ArgumentException("Query options cannot be null");

        queryOptions.AddPagination(1, 1);
        return await _dataSourceBroker.Get<TEntity>(x => true).ApplyQuery(queryOptions).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async ValueTask<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dataSourceBroker.Get<TEntity>(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async ValueTask<TEntity> CreateAsync(TEntity entity, bool save = true, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentException("Cannot create null entity", nameof(entity));

        entity.Id = default;
        await _dataSourceBroker.CreateAsync(entity, cancellationToken);
        return !save || await _dataSourceBroker.SaveAsync(cancellationToken) ? entity : throw new Exception("Failed to create entity");
    }

    public virtual async ValueTask<TEntity?> UpdateAsync(TEntity entity, bool save = true, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentException("Cannot update null entity", nameof(entity));

        _ = await GetByIdAsync(entity.Id, cancellationToken) ?? throw new EntityNotFoundException();
        await _dataSourceBroker.UpdateAsync(entity, cancellationToken);
        return !save || await _dataSourceBroker.SaveAsync(cancellationToken) ? entity : throw new Exception("Failed to update entity");
    }

    public virtual async ValueTask<bool> DeleteAsync(long id, bool save = true, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken) ?? throw new EntityNotFoundException(typeof(TEntity).Name, id.ToString());
        await _dataSourceBroker.DeleteAsync(entity, cancellationToken);
        return !save || await _dataSourceBroker.SaveAsync(cancellationToken);
    }
}