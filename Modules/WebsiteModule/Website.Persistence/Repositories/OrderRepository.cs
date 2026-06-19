using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, Application.Contracts.Persistence.Repositories.IOrderRepository
    {
        public OrderRepository(WebsiteDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<AdminOrderListDto>> GetAdminOrdersPagedAsync(
            AdminOrderFilter filter,
           
            CancellationToken cancellationToken)
        {
            var query = _dbSet.AsNoTracking();

            if (filter.Status.HasValue)
            {
                query = query.Where(o => o.Status == filter.Status.Value);
            }

            if (filter.DateFrom.HasValue)
            {
                query = query.Where(o => o.OrderDate >= filter.DateFrom.Value);
            }

            if (filter.DateTo.HasValue)
            {
                query = query.Where(o => o.OrderDate <= filter.DateTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(o => o.OrderNumber.Contains(filter.SearchTerm));
            }

            if (filter.MinAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount >= filter.MinAmount.Value);
            }

            if (filter.MaxAmount.HasValue)
            {
                query = query.Where(o => o.TotalAmount <= filter.MaxAmount.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(o => new AdminOrderListDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Status = o.Status,
                    SubTotal = o.SubTotal,
                    DiscountTotal = o.DiscountTotal,
                    TotalAmount = o.TotalAmount,
                    ItemCount = o.Items.Count, // Calculated via aggregation in DB
                    OrderDate = o.OrderDate,
                    UserId = o.UserId,
                    CouponCode = o.AppliedCouponCode
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<AdminOrderListDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<OrderDetailsDto?> GetAdminOrderDetailsAsync(
            Guid orderId,
           
            CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking()
                .Where(o => o.Id == orderId)
                .Select(o => new OrderDetailsDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Status = o.Status,
                    SubTotal = o.SubTotal,
                    DiscountTotal = o.DiscountTotal,
                    CouponDiscountAmount = o.CouponDiscountAmount,
                    TotalAmount = o.TotalAmount,
                    OrderDate = o.OrderDate,
                    UserId = o.UserId,
                    CustomerName = o.CustomerName,
                    CustomerPhone = o.CustomerPhone,
                    PaymentMethod = o.PaymentMethod,
                    CouponCode = o.AppliedCouponCode,
                    Notes = o.Notes,
                    ShippingDetails = new ShippingDetailsDto
                    {
                        RecipientName = o.ShippingAddress.RecipientName,
                        Phone = o.ShippingAddress.Phone,
                        Street = o.ShippingAddress.Street,
                        City = o.ShippingAddress.City,
                        State = o.ShippingAddress.State,
                        Country = o.ShippingAddress.Country,
                        ZipCode = o.ShippingAddress.ZipCode
                    },
                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        ProductNameSnapshot = i.ProductNameSnapshot,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        DiscountAmount = i.DiscountAmount,
                        FinalPrice = i.FinalPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PagedResult<UserOrderListDto>> GetUserOrdersPagedAsync(
            string userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var query = _dbSet.AsNoTracking().Where(o => o.UserId == userId);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new UserOrderListDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    OrderDate = o.OrderDate,
                    ItemCount = o.Items.Count
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<UserOrderListDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<UserOrderDetailsDto?> GetUserOrderDetailsAsync(
            Guid orderId,
            string userId,
            CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking()
                .Where(o => o.Id == orderId && o.UserId == userId)
                .Select(o => new UserOrderDetailsDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Status = o.Status,
                    SubTotal = o.SubTotal,
                    DiscountTotal = o.DiscountTotal,
                    TotalAmount = o.TotalAmount,
                    OrderDate = o.OrderDate,
                    CustomerName = o.CustomerName,
                    CustomerPhone = o.CustomerPhone,
                    PaymentMethod = o.PaymentMethod,
                    Notes = o.Notes,
                    ShippingDetails = new ShippingDetailsDto
                    {
                        RecipientName = o.ShippingAddress.RecipientName,
                        Phone = o.ShippingAddress.Phone,
                        Street = o.ShippingAddress.Street,
                        City = o.ShippingAddress.City,
                        State = o.ShippingAddress.State,
                        Country = o.ShippingAddress.Country,
                        ZipCode = o.ShippingAddress.ZipCode
                    },
                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        ProductNameSnapshot = i.ProductNameSnapshot,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        DiscountAmount = i.DiscountAmount,
                        FinalPrice = i.FinalPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<AdminDashboardStatsDto> GetAdminDashboardStatsAsync(
            CancellationToken cancellationToken)
        {
            var stats = await _dbSet.AsNoTracking()
                .GroupBy(o => 1)
                .Select(g => new AdminDashboardStatsDto
                {
                    TotalRevenue = g.Where(o => o.Status == OrderStatus.Completed)
                                    .Sum(o => (decimal?)o.TotalAmount) ?? 0,
                    ProcessingOrdersCount = g.Count(o => o.Status == OrderStatus.Processing),
                    CompletedOrdersCount = g.Count(o => o.Status == OrderStatus.Completed),
                    CancelledOrdersCount = g.Count(o => o.Status == OrderStatus.Cancelled)
                })
                .FirstOrDefaultAsync(cancellationToken);

            return stats ?? new AdminDashboardStatsDto();
        }

        public async Task<Order?> GetOrderForAnalyticsAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }

        public async Task<string?> GetFavoritePurchaseDayAsync(string userId, CancellationToken cancellationToken)
        {
            var dates = await _context.Orders
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .Select(o => o.OrderDate)
                .ToListAsync(cancellationToken);

            if (!dates.Any())
                return null;

            var favoriteDay = dates
                .GroupBy(d => d.DayOfWeek)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            return favoriteDay.ToString();
        }
        public async Task<string?> GetMostPurchasedCategoryAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.OrderItems.AsNoTracking()
                .Where(i => i.Order.UserId == userId)
                .GroupBy(i => i.Product.Category != null ? i.Product.Category.Name : "Uncategorized")
                .OrderByDescending(g => g.Sum(i => i.Quantity))
                .Select(g => g.Key)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
