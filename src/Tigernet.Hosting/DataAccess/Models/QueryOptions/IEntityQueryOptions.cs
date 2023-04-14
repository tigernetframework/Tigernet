using Tigernet.Hosting.DataAccess.Models.Entity;

namespace Tigernet.Hosting.DataAccess.Models.QueryOptions;

/// <summary>
/// Defines properties for queryable entities source query options
/// </summary>
/// <typeparam name="TEntity">Query source type</typeparam>
public interface IEntityQueryOptions<TEntity> : IQueryOptions<TEntity> where TEntity : class, IQueryableEntity
{
    /// <summary>
    /// Requested include model options
    /// </summary>
    IncludeOptions<TEntity>? IncludeOptions { get; set; }
}