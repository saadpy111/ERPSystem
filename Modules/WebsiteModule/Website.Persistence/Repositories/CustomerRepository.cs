using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;
using Website.Domain.Entities;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly WebsiteDbContext _context;

        public CustomerRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CustomerProfile profile, CancellationToken cancellationToken = default)
        {
            await _context.CustomerProfiles.AddAsync(profile, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<CustomerListDto>> GetCustomersPagedAsync(
            CustomerFilter filter,
            IEnumerable<string>? userIds,
            CancellationToken cancellationToken)
        {
            var query = _context.CustomerProfiles.AsNoTracking();

            if (userIds != null)
                query = query.Where(c => userIds.Contains(c.UserId));

            if (filter.JoinedFrom.HasValue)
                query = query.Where(c => c.CustomerSince >= filter.JoinedFrom.Value);

            if (filter.JoinedTo.HasValue)
                query = query.Where(c => c.CustomerSince <= filter.JoinedTo.Value);

            var joinedQuery =
                from c in query
                join a in _context.CustomerAnalytics.AsNoTracking()
                    on c.UserId equals a.UserId into analytics
                from ca in analytics.DefaultIfEmpty()
                select new
                {
                    c.Id,
                    c.UserId,
                    c.AvatarUrl,
                    c.CustomerSince,

                    OrdersCount = ca != null ? ca.OrdersCount : 0,
                    TotalSpent = ca != null ? ca.TotalSpent : 0m,
                    LastOrderDate = ca != null ? ca.LastOrderDate : null
                };

            if (filter.MinOrdersCount.HasValue)
                joinedQuery = joinedQuery.Where(c => c.OrdersCount >= filter.MinOrdersCount.Value);

            var totalCount = await joinedQuery.CountAsync(cancellationToken);

            var items = await joinedQuery
                .OrderByDescending(c => c.CustomerSince)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(c => new CustomerListDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    AvatarUrl = c.AvatarUrl,
                    OrdersCount = c.OrdersCount,
                    TotalSpent = c.TotalSpent,
                    LastOrderDate = c.LastOrderDate,
                    CustomerSince = c.CustomerSince,

                    FullName = string.Empty,
                    Email = string.Empty
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<CustomerListDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<CustomerDetailsDto?> GetCustomerDetailsAsync(
            Guid customerId,
            int ordersPage,
            int ordersPageSize,
            CancellationToken cancellationToken)
        {
            var profile = await (
                from c in _context.CustomerProfiles.AsNoTracking()
                join a in _context.CustomerAnalytics.AsNoTracking()
                    on c.UserId equals a.UserId into analytics
                from ca in analytics.DefaultIfEmpty()
                where c.Id == customerId
                select new
                {
                    c.Id,
                    c.UserId,
                    c.AvatarUrl,
                    c.Address,
                    c.City,
                    c.Country,
                    c.PostalCode,
                    c.BirthDate,
                    c.CustomerSince,

                    OrdersCount = ca != null ? ca.OrdersCount : 0,
                    TotalSpent = ca != null ? ca.TotalSpent : 0m,
                    AverageOrderValue = ca != null ? ca.AverageOrderValue : 0m,
                    LastOrderDate = ca != null ? ca.LastOrderDate : null,
                    FirstOrderDate = ca != null ? ca.FirstOrderDate : null,
                    MostPurchasedCategory = ca != null ? ca.MostPurchasedCategory : null,

                    AverageDaysBetweenOrders = ca != null ? ca.AverageDaysBetweenOrders : 0,
                    FavoritePurchaseDay = ca != null ? ca.FavoritePurchaseDay : null,
                    ReturnRate = ca != null ? ca.ReturnRate : 0,
                    TotalItemsPurchased = ca != null ? ca.TotalItemsPurchased : 0,
                    ReturnedItems = ca != null ? ca.ReturnedItems : 0
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (profile == null)
                return null;

            // Order history pagination
            var ordersQuery = _context.Orders
                .AsNoTracking()
                .Where(o => o.UserId == profile.UserId);

            var ordersTotalCount = await ordersQuery.CountAsync(cancellationToken);

            var recentOrders = await ordersQuery
                .OrderByDescending(o => o.OrderDate)
                .Skip((ordersPage - 1) * ordersPageSize)
                .Take(ordersPageSize)
                .Select(o => new CustomerOrderSummaryDto
                {
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,

                    ItemsCount = o.Items.Sum(i => i.Quantity),

                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString()
                })
                .ToListAsync(cancellationToken);

            return new CustomerDetailsDto
            {
                Id = profile.Id,
                UserId = profile.UserId,

                AvatarUrl = profile.AvatarUrl,
                Address = profile.Address,
                City = profile.City,
                Country = profile.Country,
                PostalCode = profile.PostalCode,
                BirthDate = profile.BirthDate,
                CustomerSince = profile.CustomerSince,

                FullName = string.Empty,
                Email = string.Empty,
                PhoneNumber = null,

                // Purchase stats
                TotalOrders = profile.OrdersCount,
                TotalSpent = profile.TotalSpent,
                AverageOrderValue = profile.AverageOrderValue,
                LastOrderDate = profile.LastOrderDate,
                FirstOrderDate = profile.FirstOrderDate,
                MostPurchasedCategory = profile.MostPurchasedCategory,

                // Behavior stats
                AverageDaysBetweenOrders = profile.AverageDaysBetweenOrders,
                FavoritePurchaseDay = profile.FavoritePurchaseDay,
                ReturnRate = profile.ReturnRate,
                TotalItemsPurchased = profile.TotalItemsPurchased,
                ReturnedItems = profile.ReturnedItems,

                RecentOrders = recentOrders,
                RecentOrdersTotalCount = ordersTotalCount
            };
        }
    }
}