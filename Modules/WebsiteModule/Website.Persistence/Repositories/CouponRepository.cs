using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly WebsiteDbContext _context;

        public CouponRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<CouponListItemDto>> GetCouponsPagedAsync(
            CouponFilter filter,
            string tenantId,
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var query = _context.Coupons
                .AsNoTracking()
                .Where(c => c.TenantId == tenantId);

            if (filter.IsActive.HasValue)
            {
                query = query.Where(c => c.IsActive == filter.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(term) || c.Code.ToLower().Contains(term));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            // Sorting
            if (!string.IsNullOrWhiteSpace(filter.SortColumn))
            {
                var isDesc = filter.SortOrder?.ToLower() == "desc";
                query = filter.SortColumn.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                    "code" => isDesc ? query.OrderByDescending(c => c.Code) : query.OrderBy(c => c.Code),
                    "startdate" => isDesc ? query.OrderByDescending(c => c.StartDate) : query.OrderBy(c => c.StartDate),
                    "enddate" => isDesc ? query.OrderByDescending(c => c.EndDate) : query.OrderBy(c => c.EndDate),
                    _ => query.OrderByDescending(c => c.CreatedAt)
                };
            }
            else
            {
                query = query.OrderByDescending(c => c.CreatedAt);
            }

            var itemsToSkip = (filter.Page - 1) * filter.PageSize;

            var items = await query
                .Skip(itemsToSkip)
                .Take(filter.PageSize)
                .Select(c => new CouponListItemDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                    DiscountType = c.DiscountType,
                    DiscountValue = c.DiscountValue,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Status = !c.IsActive ? "Inactive" : c.EndDate < now ? "Expired" : "Active",
                    UsageCount = c.Usages.Count,
                    UsageLimit = c.UsageLimit,
                    UsagePercentage = c.UsageLimit.HasValue && c.UsageLimit.Value > 0
                        ? (double)c.Usages.Count / c.UsageLimit.Value * 100
                        : null,
                    MinimumOrderAmount = c.MinimumOrderAmount
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<CouponListItemDto>
            {
                TotalCount = totalCount,
                Items = items,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<CouponDetailsDto?> GetCouponDetailsByIdAsync(
            Guid couponId,
            string tenantId,
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await _context.Coupons
                .AsNoTracking()
                .Where(c => c.Id == couponId && c.TenantId == tenantId)
                .Select(c => new CouponDetailsDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                    DiscountType = c.DiscountType,
                    DiscountValue = c.DiscountValue,
                    IsActive = c.IsActive,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    DaysRemaining = c.EndDate > now ? (c.EndDate - now).Days : 0,
                    Status = !c.IsActive ? "Inactive" : c.EndDate < now ? "Expired" : "Active",
                    UsageCount = c.Usages.Count,
                    UsageLimit = c.UsageLimit,
                    UsagePerUserLimit = c.UsagePerUserLimit,
                    UsagePercentage = c.UsageLimit.HasValue && c.UsageLimit.Value > 0
                        ? (double)c.Usages.Count / c.UsageLimit.Value * 100
                        : null,
                    OrdersCount = c.Usages.Select(u => u.OrderId).Distinct().Count(),
                    UniqueUsersCount = c.Usages.Select(u => u.UserId).Distinct().Count(),
                    TotalDiscountAmountGiven = c.Usages.Sum(u => (decimal?)u.Order.CouponDiscountAmount) ?? 0,
                    TotalRevenueGenerated = c.Usages.Sum(u => (decimal?)u.Order.TotalAmount) ?? 0,
                    AverageOrderValue = c.Usages.Any() 
                        ? c.Usages.Average(u => u.Order.TotalAmount) 
                        : 0
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
