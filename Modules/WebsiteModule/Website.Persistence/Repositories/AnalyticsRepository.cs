using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly WebsiteDbContext _context;

        public AnalyticsRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProductRevenueDto>> GetProductRevenuePagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var baseQuery = _context.Set<OrderItem>().AsNoTracking();

            var totalRevenue = await baseQuery.SumAsync(oi => oi.FinalPrice * oi.Quantity, cancellationToken);
            var safeTotalRevenue = totalRevenue == 0 ? 1 : totalRevenue;

            var groupedQuery = baseQuery
                .GroupBy(oi => new { oi.ProductId, oi.Product.NameSnapshot })
                .Select(g => new ProductRevenueDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.NameSnapshot,
                    OrdersCount = g.Select(x => x.OrderId).Distinct().Count(),
                    TotalRevenue = g.Sum(x => x.FinalPrice * x.Quantity)
                });

            var totalCount = await groupedQuery.CountAsync(cancellationToken);

            var items = await groupedQuery
                .OrderByDescending(x => x.TotalRevenue)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                item.RevenuePercentage = (item.TotalRevenue / safeTotalRevenue) * 100m;
            }

            return new PagedResult<ProductRevenueDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<CategoryRevenueDto>> GetCategoryRevenuePagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var baseQuery = _context.Set<OrderItem>().AsNoTracking();

            var totalRevenue = await baseQuery.SumAsync(oi => oi.FinalPrice * oi.Quantity, cancellationToken);
            var safeTotalRevenue = totalRevenue == 0 ? 1 : totalRevenue;

            var groupedQuery = baseQuery
                .GroupBy(oi => new { oi.Product.CategoryId, oi.Product.Category!.Name })
                .Select(g => new CategoryRevenueDto
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.Name,
                    TotalRevenue = g.Sum(x => x.FinalPrice * x.Quantity),
                    ProductsCount = g.Select(x => x.ProductId).Distinct().Count()
                });

            var totalCount = await groupedQuery.CountAsync(cancellationToken);

            var items = await groupedQuery
                .OrderByDescending(x => x.TotalRevenue)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                item.RevenuePercentage = (item.TotalRevenue / safeTotalRevenue) * 100m;
            }

            return new PagedResult<CategoryRevenueDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<DashboardKpiDto> GetDashboardKpisAsync(
                DateTime? fromDate,
                DateTime? toDate,
                CancellationToken cancellationToken = default)

        {
            var now = DateTime.UtcNow;

            var end = toDate?.ToUniversalTime() ?? now;
            var start = fromDate?.ToUniversalTime() ?? end.AddDays(-30);

            var intervalDays = (end - start).Days;
            var previousStart = start.AddDays(-intervalDays);

            var stats = await _context.Set<Order>()
                .AsNoTracking()
                .Where(o => o.OrderDate >= previousStart && o.OrderDate < end)
                .GroupBy(o => 1)
                .Select(g => new
                {
                    TotalOrders = g.Count(o => o.OrderDate >= start),

                    CompletedOrders = g.Count(o =>
                        o.OrderDate >= start &&
                        o.Status == OrderStatus.Completed),

                    ReturnedOrders = g.Count(o =>
                        o.OrderDate >= start &&
                        o.Status == OrderStatus.Returned),

                    GrossRevenue = g.Sum(o =>
                        o.OrderDate >= start &&
                        o.Status == OrderStatus.Completed
                            ? o.SubTotal
                            : 0),

                    NetRevenue = g.Sum(o =>
                        o.OrderDate >= start &&
                        o.Status == OrderStatus.Completed
                            ? o.TotalAmount
                            : 0),

                    TotalDiscounts = g.Sum(o =>
                        o.OrderDate >= start
                            ? o.DiscountTotal
                            : 0),

                    PrevRevenue = g.Sum(o =>
                        o.OrderDate < start &&
                        o.Status == OrderStatus.Completed
                            ? o.TotalAmount
                            : 0)
                })
                .FirstOrDefaultAsync(cancellationToken);

            var kpi = new DashboardKpiDto();

            if (stats != null)
            {
                kpi.PaidOrders = stats.CompletedOrders;
                kpi.GrossRevenue = stats.GrossRevenue;
                kpi.NetRevenue = stats.NetRevenue;
                kpi.TotalDiscounts = stats.TotalDiscounts;

                if (stats.TotalOrders > 0)
                    kpi.PaymentSuccessRate =
                        (stats.CompletedOrders / (decimal)stats.TotalOrders) * 100m;

                if (stats.CompletedOrders > 0)
                {
                    kpi.AverageOrderValue =
                        stats.NetRevenue / stats.CompletedOrders;

                    kpi.ReturnRate =
                        (stats.ReturnedOrders / (decimal)stats.CompletedOrders) * 100m;
                }

                if (stats.PrevRevenue > 0)
                    kpi.GrowthRate =
                        ((stats.NetRevenue - stats.PrevRevenue) / stats.PrevRevenue) * 100m;
                else if (stats.NetRevenue > 0)
                    kpi.GrowthRate = 100m;
            }

            return kpi;
        }
    }
}
