using System.Linq.Expressions;
using Tigernet.Hosting.DataAccess.Entity;
using Tigernet.Hosting.DataAccess.Query;
using Tigernet.Hosting.DataAccess.QueryOptions;

namespace Tigernet.Hosting.DataAccess.EntityManager;

/// <summary>
/// Defines base clever interface
/// </summary>
public interface IEntityManagerBase<TEntity> where TEntity : class, IEntity, IQueryableEntity
{
    /// <summary>
    /// Gets entity set as queryable collection without decryption
    /// </summary>
    /// <param name="expression">Predicate expression</param>
    /// <returns>Queryable collection of entity</returns>
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// Gets entity set after querying
    /// </summary>
    /// <param name="expression">Predicate expression</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Set of entities</returns>
    ValueTask<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a set of entities by filter
    /// </summary>
    /// <param name="queryOptions">Complex query options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Set of entities after query</returns>
    ValueTask<IEnumerable<TEntity>> GetAsync(IEntityQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a set of entities by filter
    /// </summary>
    /// <param name="queryOptions">Complex query options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Set of entities after query</returns>
    ValueTask<TEntity?> GetFirstAsync(IEntityQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets entity by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity if found, otherwise null</returns>
    ValueTask<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds entity in context scope or saves it to database
    /// </summary>
    /// <param name="entity">Entity being created</param>
    /// <param name="save">Determines whether to commit changes to database</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Created entity</returns>
    ValueTask<TEntity> CreateAsync(TEntity entity, bool save = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates entity in context scope or saves it to database
    /// </summary>
    /// <param name="entity">Entity being updated</param>
    /// <param name="save">Determines whether to commit changes to database</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>True if succeeded, otherwise false</returns>
    ValueTask<bool> UpdateAsync(TEntity entity, bool save = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes entity in context scope or saves it to database
    /// </summary>
    /// <param name="entity">Entity being deleted</param>
    /// <param name="save">Determines whether to commit changes to database</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>True if succeeded, otherwise false</returns>
    ValueTask<bool> DeleteAsync(TEntity entity, bool save = false, CancellationToken cancellationToken = default);
}