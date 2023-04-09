using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Tigernet.Hosting.DataAccess.Entity;

public interface IEntitySet<TEntity> where TEntity : class, IEntity
{
    // List<IEntitySnapshot<TEntity>> EntitySnapshots { get; }

    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression);

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);
}