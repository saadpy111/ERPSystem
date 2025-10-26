using Identity.Application.Contracts.Persistence;
using Identity.Application.Pagination;
using Identity.Persistense.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Persistense.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IdentityDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }



        public async Task<T?> GetById(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }


        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<List<T>> GetAll
               (Expression<Func<T, bool>>? filter, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);


            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<bool> Any(Expression<Func<T, bool>>? filter = null)
        {
            return filter == null ? await _dbSet.AnyAsync() : await _dbSet.AnyAsync(filter);
        }

        public async Task<T?> GetFirst(
        Expression<Func<T, bool>> filter,
        bool asNoTracking = false,
        params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<PagedResult<T>?> Search(Expression<Func<T, bool>>? filter, int pagenom, int pagesize, Func<IQueryable<T>, IOrderedQueryable<T>>? orderby = null, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (include != null)
                query = include(query);

            if (filter != null)
                query = query.Where(filter);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagenom - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
