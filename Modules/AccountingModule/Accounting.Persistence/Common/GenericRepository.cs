using Accounting.Application.Interfaces.Repositories;
using Accounting.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Accounting.Persistence.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AccountingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AccountingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
            // Example of soft delete if applicable:
            // var property = entity.GetType().GetProperty("IsDeleted");
            // if (property != null)
            // {
            //     property.SetValue(entity, true);
            //     _dbSet.Update(entity);
            // }
            // else
            // {
            //     _dbSet.Remove(entity);
            // }
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Query() is exposed for reporting purposes only and should not be used in domain/business logic.
        /// </summary>
        public virtual IQueryable<T> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}
