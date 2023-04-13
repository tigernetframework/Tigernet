using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tigernet.Hosting.Models.Common;

namespace Tigernet.Hosting.DataAccess.Generics
{
    public interface IRepository<TEntity, TKey> where TEntity : Auditable<TKey>
    {
        IQueryable<TEntity> SelectAll();
        ValueTask<TEntity> SelectAsync(Expression<Func<TEntity, bool>> expression = null);
        ValueTask<TEntity> InsertAsync(TEntity entity);
        ValueTask<TEntity> UpdateAsync(TEntity entity);
        ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression);
    }
}