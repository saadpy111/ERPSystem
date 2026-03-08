using System;
using System.Linq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly WebsiteDbContext _context;

        public AdminDashboardRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardOverviewDto> GetDashboardOverviewAsync(int? days, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
        {
            var dto = new DashboardOverviewDto();

            DateTime start;
            DateTime end;

            if (fromDate.HasValue && toDate.HasValue)
            {
                start = fromDate.Value.ToUniversalTime();
                end = toDate.Value.ToUniversalTime();
            }
            else
            {
                var intervalDays = days ?? 30; // default to 30 days
                end = DateTime.UtcNow;
                start = end.AddDays(-intervalDays);
            }

            var ordersQuery = _context.Set<Order>().AsNoTracking()
                .Where(o => o.OrderDate >= start && o.OrderDate <= end);

            var ordersStats = await ordersQuery
                .GroupBy(o => 1)
                .Select(g => new
                {
                    TotalSales = g.Sum(o => o.Status == OrderStatus.Completed ? o.TotalAmount : 0),
                    TotalOrders = g.Sum(o => o.Status == OrderStatus.Completed ? 1 : 0),
                    PendingOrders = g.Sum(o => o.Status == OrderStatus.Pending ? 1 : 0)
                })
                .FirstOrDefaultAsync(cancellationToken);

            dto.TotalVisitors = await _context.Set<WebsiteVisitorSession>()
                .AsNoTracking()
                .Where(v => v.StartedAt >= start && v.StartedAt <= end)
                .Select(v => v.SessionId)
                .Distinct()
                .CountAsync(cancellationToken);

            dto.ActiveOffers = await _context.Set<Offer>()
                .AsNoTracking()
                .Where(o => o.IsActive)
                .CountAsync(cancellationToken);

            if (ordersStats != null)
            {
                dto.TotalSales = ordersStats.TotalSales;
                dto.TotalOrders = ordersStats.TotalOrders;
                dto.PendingOrders = ordersStats.PendingOrders;

                if (dto.TotalOrders > 0)
                {
                    dto.AverageOrderValue = dto.TotalSales / dto.TotalOrders;
                }
            }


            return dto;
        }
        public async Task<List<WeeklyAnalyticsDto>> GetWeeklyOverviewAsync(
            int? days,
            CancellationToken cancellationToken = default)
        {
            var intervalDays = days ?? 7;

            var end = DateTime.UtcNow.Date;
            var start = end.AddDays(-(intervalDays - 1));

            var ordersStats = await _context.Set<Order>()
                .AsNoTracking()
                .Where(o => o.OrderDate >= start &&
                            o.OrderDate < end.AddDays(1) &&
                            o.Status == OrderStatus.Completed)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Sales = g.Sum(o => o.TotalAmount),
                    Orders = g.Count()
                })
                .ToListAsync(cancellationToken);

            var visitorsStats = await _context.Set<WebsiteVisitorSession>()
                .AsNoTracking()
                .Where(v => v.StartedAt >= start &&
                            v.StartedAt < end.AddDays(1))
                .GroupBy(v => v.StartedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Visitors = g.Select(v => v.SessionId).Distinct().Count()
                })
                .ToListAsync(cancellationToken);

            var ordersDict = ordersStats.ToDictionary(x => x.Date);
            var visitorsDict = visitorsStats.ToDictionary(x => x.Date);

            var result = new List<WeeklyAnalyticsDto>(intervalDays);

            for (int i = 0; i < intervalDays; i++)
            {
                var date = start.AddDays(i);
                var day = date.DayOfWeek;

                ordersDict.TryGetValue(date, out var orderStat);
                visitorsDict.TryGetValue(date, out var visitorStat);

                result.Add(new WeeklyAnalyticsDto
                {
                    DayOfWeek = day.ToString(),
                    Sales = orderStat?.Sales ?? 0,
                    Orders = orderStat?.Orders ?? 0,
                    Visitors = visitorStat?.Visitors ?? 0
                });
            }

            return result;
        }
    }
}
