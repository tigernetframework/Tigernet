using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tigernet.Hosting.Models.Common;

namespace Tigernet.Hosting.DataAccess.Generics
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Auditable<TKey>
    {
        public ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TEntity> InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> SelectAll()
        {
            throw new NotImplementedException();
        }

        public ValueTask<TEntity> SelectAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TEntity> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}