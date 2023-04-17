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
    private readonly IDataStorageBroker _dataStorageBroker;

    public EntityManagerBaseService(IDataStorageBroker dataStorageBroker)
    {
        _dataStorageBroker = dataStorageBroker;
    }

    public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression)
    {
        return _dataStorageBroker.Select(expression);
    }

    public virtual ValueTask<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        if (expression is null)
            throw new ArgumentException("Query expression cannot be null");

        return new ValueTask<IEnumerable<TEntity>>(_dataStorageBroker.Select(expression).ToList());
    }

    public virtual async ValueTask<IEnumerable<TEntity>> GetAsync(
        IEntityQueryOptions<TEntity> queryOptions,
        CancellationToken cancellationToken = default
    )
    {
        if (queryOptions is null)
            throw new ArgumentException("Query options cannot be null");

        Expression<Func<TEntity, bool>> initialPredicate = x => true;
        return await _dataStorageBroker.Select<TEntity>(x => true).ApplyQuery(queryOptions).ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async ValueTask<TEntity?> GetFirstAsync(IEntityQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken = default)
    {
        if (queryOptions is null)
            throw new ArgumentException("Query options cannot be null");

        queryOptions.AddPagination(1, 1);
        return await _dataStorageBroker.Select<TEntity>(x => true).ApplyQuery(queryOptions).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async ValueTask<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dataStorageBroker.Select<TEntity>(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async ValueTask<bool> CheckByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dataStorageBroker.CheckByIdAsync<TEntity>(id, cancellationToken);
    }

    public virtual async ValueTask<TEntity?> CreateAsync(TEntity entity, bool save = true, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentException("Cannot create null entity", nameof(entity));

        entity.Id = default;
        await _dataStorageBroker.InsertAsync(entity, cancellationToken);
        return !save || await _dataStorageBroker.SaveAsync(cancellationToken) ? entity : throw new Exception("Failed to create entity");
    }

    public virtual async ValueTask<TEntity?> UpdateAsync(TEntity entity, bool save = true, CancellationToken cancellationToken = default)
    {
        if (entity is null)
            throw new ArgumentException("Cannot update null entity", nameof(entity));

        if (!await CheckByIdAsync(entity.Id, cancellationToken))
            throw new EntityNotFoundException();
        await _dataStorageBroker.UpdateAsync(entity, cancellationToken);
        return !save || await _dataStorageBroker.SaveAsync(cancellationToken) ? entity : throw new Exception("Failed to update entity");
    }


    public virtual async ValueTask<bool> DeleteAsync(long id, bool save = true, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken) ?? throw new EntityNotFoundException(typeof(TEntity).Name, id.ToString());
        await _dataStorageBroker.DeleteAsync(entity, cancellationToken);
        return !save || await _dataStorageBroker.SaveAsync(cancellationToken);
    }
}