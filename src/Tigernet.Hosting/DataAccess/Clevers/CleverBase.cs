using System.Linq.Expressions;
using Tigernet.Hosting.Extensions;
using Tigernet.Hosting.Models.Common;
using Tigernet.Hosting.Models.Query;

namespace Tigernet.Hosting.DataAccess.Clevers;

/// <summary>
/// Provides base clever implementation
/// </summary>
/// <typeparam name="TEntity">Data type</typeparam>
/// <typeparam name="TKey">Data Id type</typeparam>
public class CleverBase<TEntity, TKey> : ICleverBase<TEntity, TKey> where TEntity : class, IEntity<TKey>, IQueryableEntity where TKey : struct
{
    public IQueryable<TEntity> DataSource { init; get; }

    public CleverBase()
    {
        DataSource = Enumerable.Empty<TEntity>().AsQueryable();
    }

    public CleverBase(IQueryable<TEntity> dataSource)
    {
        DataSource = dataSource;
    }

    public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression)
    {
        return DataSource.Where(expression);
    }

    public virtual ValueTask<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default
    )
    {
        if (expression is null)
            throw new ArgumentException("Query expression cannot be null");

        return new ValueTask<IEnumerable<TEntity>>(DataSource.Where(expression).ToList());
    }

    public virtual ValueTask<IEnumerable<TEntity>> GetAsync(IEntityQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken = default)
    {
        if (queryOptions is null)
            throw new ArgumentException("Query options cannot be null");

        return new ValueTask<IEnumerable<TEntity>>(DataSource.Where(x => true).ApplyQuery(queryOptions).ToList());
    }

    public virtual ValueTask<TEntity?> GetFirstAsync(IEntityQueryOptions<TEntity> queryOptions, CancellationToken cancellationToken = default)
    {
        if (queryOptions is null)
            throw new ArgumentException("Query options cannot be null");

        queryOptions.AddPagination(1, 1);
        return new ValueTask<TEntity?>(DataSource.Where(x => true).ApplyQuery(queryOptions).FirstOrDefault());
    }

    public virtual ValueTask<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return new ValueTask<TEntity?>(Task.FromResult(DataSource.FirstOrDefault(x => x.Id.Equals(id))));
    }
}