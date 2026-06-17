using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    public class NewsletterSubscriberRepository : INewsletterSubscriberRepository
    {
        private readonly WebsiteDbContext _context;

        public NewsletterSubscriberRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<NewsletterSubscriber?> GetByIdAsync(Guid id)
        {
            return await _context.Set<NewsletterSubscriber>().FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<NewsletterSubscriber?> GetByEmailAsync(string email)
        {
            return await _context.Set<NewsletterSubscriber>()
                .FirstOrDefaultAsync(n => n.Email == email);
        }

        public async Task<(List<NewsletterSubscriber> Items, int TotalCount)> GetPagedAsync(
            string? search, bool? isActive, int pageNumber, int pageSize)
        {
            var query = _context.Set<NewsletterSubscriber>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(n => n.Email.Contains(search));

            if (isActive.HasValue)
                query = query.Where(n => n.IsActive == isActive.Value);

            query = query.OrderByDescending(n => n.SubscribedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<NewsletterSubscriber> AddAsync(NewsletterSubscriber subscriber)
        {
            await _context.Set<NewsletterSubscriber>().AddAsync(subscriber);
            return subscriber;
        }

        public void Update(NewsletterSubscriber subscriber)
        {
            _context.Set<NewsletterSubscriber>().Update(subscriber);
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Set<NewsletterSubscriber>().AnyAsync(n => n.Email == email);
        }
    }
}
