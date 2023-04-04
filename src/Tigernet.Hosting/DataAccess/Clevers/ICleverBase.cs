using System.Linq.Expressions;
using Tigernet.Hosting.Models.Common;
using Tigernet.Hosting.Models.Query;

namespace Tigernet.Hosting.DataAccess.Clevers;

/// <summary>
/// Defines base clever interface
/// </summary>
public interface ICleverBase<TEntity, TKey> where TEntity : class, IEntity<TKey>, IQueryableEntity where TKey : struct
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
    ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
}