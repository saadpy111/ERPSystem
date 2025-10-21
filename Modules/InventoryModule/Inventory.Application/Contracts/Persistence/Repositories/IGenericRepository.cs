using Inventory.Application.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Contracts.Persistence.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        #region Commands
        public Task Add(T entity);
        public void Remove(T entity);
        public void Update(T entity);
        #endregion

        #region Queries
      

        public Task<bool> Any(Expression<Func<T, bool>>? filter = null );
        public Task<T?> GetFirst(Expression<Func<T, bool>> filter ,   bool asNoTracking = false, params Expression<Func<T, object>>[] includes);
        public Task<T?> GetById(Guid id , params Expression<Func<T,object>>[] includes);
        public Task<PagedResult<T>?> Search
                   (  Expression<Func<T,bool>>? filter ,
                      int pagenom,
                      int pagesize,
                      Func<IQueryable<T> ,IOrderedQueryable<T>>? orderby =null,

                      params Expression<Func<T, object>>[] includes
                   );

        public Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);
        #endregion



    }
}
