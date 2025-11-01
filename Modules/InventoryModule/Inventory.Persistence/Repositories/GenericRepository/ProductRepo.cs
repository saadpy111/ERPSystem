using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Application.Pagination;
using Inventory.Domain.Entities;
using Inventory.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Persistence.Repositories.GenericRepository
{
    public class ProductRepo : IproductRepo
    {
        private readonly InventoryDbContext _context;
        private DbSet<Product> _dbSet;
        public ProductRepo(InventoryDbContext context)
        {
              _context = context;
            _dbSet = _context.Products;
        }
        public async Task<PagedResult<Product>?>

        SearchProducts
            (
               Expression<Func<Product, bool>>? filter,
               int pagenom, 
               int pagesize, 
               Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderby = null
            )
        {
            IQueryable<Product> query = _dbSet.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            
                query = query.Include(p=>p.AttributeValues)
                               .ThenInclude(a=>a.Attribute)
                               .Include(p => p.Category)
                               .Include(p=>p.StockQuants)
                               .ThenInclude(s=>s.Location)
                               .ThenInclude(l =>l.Warehouse)
                               .Include(p=>p.Images);                             

            if (orderby != null)
                query = orderby(query);

            pagenom = pagenom < 1 ? 1 : pagenom;
            pagesize = pagesize <= 0 ? 10 : pagesize;

            int count = await query.CountAsync();

            var items = await query
                                .Skip((pagenom - 1) * pagesize)
                                .Take(pagesize)
                                .ToListAsync();

            return new PagedResult<Product>
            {
                Items = items,
                TotalCount = count
            };
        }





        public async Task<Product?> GetProductDetailsById(Guid id)
        {
            IQueryable<Product> query = _dbSet.AsNoTracking();


            query = query.Include(p => p.AttributeValues)
                           .ThenInclude(a => a.Attribute)
                           .Include(p => p.Category)
                           .Include(p => p.StockQuants)
                           .ThenInclude(s => s.Location)
                           .ThenInclude(l => l.Warehouse)
                           .Include(p => p.Images);

            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

    }
}
