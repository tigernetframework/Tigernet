using System.Linq.Expressions;
using Tigernet.Hosting.DataAccess.Models.Entity;

namespace Tigernet.Hosting.DataAccess.Brokers;

public interface IDataSourceBroker
{
    /// <summary>
    /// Gets entity set as queryable collection without decryption
    /// </summary>
    /// <param name="expression">Predicate expression</param>
    /// <returns>Queryable collection of entity</returns>
    IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IEntity;

    /// <summary>
    /// Adds entity in context scope or saves it to database
    /// </summary>
    /// <param name="entity">Entity being created</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Created entity if succeeded, otherwise null</returns>
    ValueTask<TEntity?> CreateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity;

    /// <summary>
    /// Updates entity in context scope or saves it to database
    /// </summary>
    /// <param name="entity">Entity being updated</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Updated entity if succeeded, otherwise null</returns>
    ValueTask<TEntity?> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity;

    /// <summary>
    /// Deletes entity in context scope or saves it to database
    /// </summary>
    /// <param name="entity">Entity being deleted</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Deleted entity if succeeded, otherwise null</returns>
    ValueTask<TEntity?> DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class?, IEntity?;

    /// <summary>
    /// Saves changes to data source
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if saved successfully, otherwise false</returns>
    ValueTask<bool> SaveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets registered entity types
    /// </summary>
    /// <returns>Set of registered entity types</returns>
    IReadOnlyList<Type> GetEntityTypes();
}