using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence
{
    public interface INewsletterSubscriberRepository
    {
        Task<NewsletterSubscriber?> GetByIdAsync(Guid id);
        Task<NewsletterSubscriber?> GetByEmailAsync(string email);
        Task<(List<NewsletterSubscriber> Items, int TotalCount)> GetPagedAsync(string? search, bool? isActive, int pageNumber, int pageSize);
        Task<NewsletterSubscriber> AddAsync(NewsletterSubscriber subscriber);
        void Update(NewsletterSubscriber subscriber);
        Task<bool> ExistsAsync(string email);
    }
}
