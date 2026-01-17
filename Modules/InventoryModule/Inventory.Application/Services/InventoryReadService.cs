using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Domain.Entities;
using SharedKernel.Contracts;

namespace Inventory.Application.Services
{
    /// <summary>
    /// Implements IInventoryReadService for cross-module read access to Inventory data.
    /// </summary>
    public class InventoryReadService : IInventoryReadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IproductRepo _productRepo;

        public InventoryReadService(IUnitOfWork unitOfWork, IproductRepo productRepo)
        {
            _unitOfWork = unitOfWork;
            _productRepo = productRepo;
        }

        public async Task<InventoryProductDto?> GetProductByIdAsync(Guid inventoryProductId)
        {
            var product = await _productRepo.GetProductDetailsById(inventoryProductId);
            if (product == null) return null;

            return MapProductToDto(product);
        }

        public async Task<InventoryProductSearchResult> SearchProductsAsync(InventoryProductSearchRequest request)
        {
            var productRepo = _unitOfWork.Repositories<Product>();

            // Build filter
            System.Linq.Expressions.Expression<Func<Product, bool>>? filter = null;

            // Start with true condition
            var filters = new List<System.Linq.Expressions.Expression<Func<Product, bool>>>();

            if (request.CategoryId.HasValue)
                filters.Add(p => p.CategoryId == request.CategoryId.Value);

            if (request.IsActive.HasValue)
                filters.Add(p => p.IsActive == request.IsActive.Value);

            if (request.PriceFrom.HasValue)
                filters.Add(p => p.SalePrice >= request.PriceFrom.Value);

            if (request.PriceTo.HasValue)
                filters.Add(p => p.SalePrice <= request.PriceTo.Value);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                filters.Add(p => 
                    p.Name.ToLower().Contains(term) || 
                    (p.Sku != null && p.Sku.ToLower().Contains(term)) ||
                    (p.ProductBarcode != null && p.ProductBarcode.ToLower().Contains(term)));
            }

            // Combine filters
            if (filters.Any())
            {
                filter = filters.First();
                foreach (var f in filters.Skip(1))
                {
                    var param = filter.Parameters[0];
                    var body = System.Linq.Expressions.Expression.AndAlso(
                        filter.Body,
                        System.Linq.Expressions.Expression.Invoke(f, param));
                    filter = System.Linq.Expressions.Expression.Lambda<Func<Product, bool>>(body, param);
                }
            }

            // Build order by
            Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = request.SortBy?.ToLower() switch
            {
                "name" => request.SortDirection?.ToLower() == "desc" 
                    ? q => q.OrderByDescending(p => p.Name) 
                    : q => q.OrderBy(p => p.Name),
                "createdat" => request.SortDirection?.ToLower() == "desc"
                    ? q => q.OrderByDescending(p => p.CreatedAt)
                    : q => q.OrderBy(p => p.CreatedAt),
                "price" => request.SortDirection?.ToLower() == "desc"
                    ? q => q.OrderByDescending(p => p.SalePrice)
                    : q => q.OrderBy(p => p.SalePrice),
                _ => q => q.OrderBy(p => p.Name)
            };

            var result = await productRepo.Search(
                filter,
                request.Page,
                request.PageSize,
                orderBy,
                p => p.Category,
                p => p.Images);

            return new InventoryProductSearchResult
            {
                Items = result?.Items.Select(MapProductToDto).ToList() ?? new List<InventoryProductDto>(),
                TotalCount = result?.TotalCount ?? 0,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public async Task<InventoryCategoryDto?> GetCategoryByIdAsync(Guid inventoryCategoryId)
        {
            var categoryRepo = _unitOfWork.Repositories<ProductCategory>();
            var category = await categoryRepo.GetById(inventoryCategoryId, c => c.Products, c => c.ParentCategory);
            if (category == null) return null;

            return MapCategoryToDto(category);
        }

        public async Task<InventoryCategorySearchResult> SearchCategoriesAsync(InventoryCategorySearchRequest request)
        {
            var categoryRepo = _unitOfWork.Repositories<ProductCategory>();

            // Build filter
            System.Linq.Expressions.Expression<Func<ProductCategory, bool>>? filter = null;
            var filters = new List<System.Linq.Expressions.Expression<Func<ProductCategory, bool>>>();

            if (request.ParentCategoryId.HasValue)
                filters.Add(c => c.ParentId == request.ParentCategoryId.Value);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                filters.Add(c => c.Name.ToLower().Contains(term));
            }

            // Combine filters
            if (filters.Any())
            {
                filter = filters.First();
                foreach (var f in filters.Skip(1))
                {
                    var param = filter.Parameters[0];
                    var body = System.Linq.Expressions.Expression.AndAlso(
                        filter.Body,
                        System.Linq.Expressions.Expression.Invoke(f, param));
                    filter = System.Linq.Expressions.Expression.Lambda<Func<ProductCategory, bool>>(body, param);
                }
            }

            // Build order by
            Func<IQueryable<ProductCategory>, IOrderedQueryable<ProductCategory>>? orderBy = request.SortBy?.ToLower() switch
            {
                "name" => request.SortDirection?.ToLower() == "desc" 
                    ? q => q.OrderByDescending(c => c.Name) 
                    : q => q.OrderBy(c => c.Name),
                "createdat" => request.SortDirection?.ToLower() == "desc"
                    ? q => q.OrderByDescending(c => c.CreatedAt)
                    : q => q.OrderBy(c => c.CreatedAt),
                _ => q => q.OrderBy(c => c.Name)
            };

            var result = await categoryRepo.Search(
                filter,
                request.Page,
                request.PageSize,
                orderBy,
                c => c.Products,
                c => c.ParentCategory);

            return new InventoryCategorySearchResult
            {
                Items = result?.Items.Select(MapCategoryToDto).ToList() ?? new List<InventoryCategoryDto>(),
                TotalCount = result?.TotalCount ?? 0,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        private static InventoryProductDto MapProductToDto(Product product)
        {
            return new InventoryProductDto
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                Description = product.Description,
                UnitOfMeasure = product.UnitOfMeasure,
                SalePrice = product.SalePrice,
                CostPrice = product.CostPrice,
                IsActive = product.IsActive,
                ProductBarcode = product.ProductBarcode,
                MainSupplierName = product.MainSupplierName,
                Tax = product.Tax,
                OrderLimit = product.OrderLimit,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                MainImageUrl = product.Images?.FirstOrDefault()?.ImageUrl,
                CreatedAt = product.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = product.UpdatedAt ?? DateTime.UtcNow
            };
        }

        private static InventoryCategoryDto MapCategoryToDto(ProductCategory category)
        {
            return new InventoryCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = null, // ProductCategory has no Description
                ParentCategoryId = category.ParentId,
                ParentCategoryName = category.ParentCategory?.Name,
                IsActive = true, // ProductCategory has no IsActive, assume active
                ProductCount = category.Products?.Count ?? 0,
                CreatedAt = category.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = category.UpdatedAt ?? DateTime.UtcNow
            };
        }
    }
}

