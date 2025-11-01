using Inventory.Application.Pagination;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Contracts.Persistence.Repositories
{
    public interface IproductRepo
    {
        Task<PagedResult<Product>?> SearchProducts(
        Expression<Func<Product, bool>>? filter,
        int pagenom,
        int pagesize,
        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderby = null
            );

        Task<Product?> GetProductDetailsById(Guid id);
    }
        
}
