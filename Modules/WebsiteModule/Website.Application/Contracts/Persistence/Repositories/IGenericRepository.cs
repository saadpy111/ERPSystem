using System.Linq.Expressions;
using Website.Application.Pagination;

namespace Website.Application.Contracts.Persistence.Repositories
{
    /// <summary>
    /// Generic repository interface for CRUD operations.
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        #region Commands
        Task AddAsync(T entity);
        void Remove(T entity);
        void Update(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void RemoveRange(IEnumerable<T> entities);
        #endregion

        #region Queries
        Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
        Task<T?> GetFirstAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);
        Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<PagedResult<T>> SearchAsync(
            Expression<Func<T, bool>>? filter,
            int page,
            int pageSize,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes);
        #endregion
    }
}
