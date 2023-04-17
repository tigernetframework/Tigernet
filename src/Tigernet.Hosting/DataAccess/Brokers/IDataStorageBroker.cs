using System.Linq.Expressions;
using Tigernet.Hosting.DataAccess.Models.Entity;

namespace Tigernet.Hosting.DataAccess.Brokers;

public interface IDataStorageBroker
{
    /// <summary>
    /// Selects entity set as queryable collection without decryption
    /// </summary>
    /// <param name="expression">Predicate expression</param>
    /// <returns>Queryable collection of entity</returns>
    IQueryable<TEntity> Select<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IEntity;
    
    /// <summary>
    /// Selects entity by id
    /// </summary>
    /// <param name="id">Entity id being selected</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>Entity if found, otherwise null</returns>
    ValueTask<TEntity?> SelectByIdAsync<TEntity>(long id, CancellationToken cancellationToken = default) where TEntity : class, IEntity;
    
    /// <summary>
    /// Checks entity by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity if found, otherwise null</returns>
    ValueTask<bool> CheckByIdAsync<TEntity>(long id, CancellationToken cancellationToken = default) where TEntity : class, IEntity;

    /// <summary>
    /// Inserts entity in context scope or saves it to database
    /// </summary>
    /// <param name="entity">Entity being created</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>Created entity if succeeded, otherwise null</returns>
    ValueTask<TEntity?> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity;

    /// <summary>
    /// Updates entity in context scope or saves it to database
    /// </summary>
    /// <param name="entity">Entity being updated</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>Updated entity if succeeded, otherwise null</returns>
    ValueTask<TEntity?> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class, IEntity;

    /// <summary>
    /// Deletes entity in context scope or saves it to database
    /// </summary>
    /// <param name="entity">Entity being deleted</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
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