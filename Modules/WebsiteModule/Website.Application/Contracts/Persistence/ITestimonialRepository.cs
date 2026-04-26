using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence
{
    public interface ITestimonialRepository
    {
        Task<Testimonial?> GetByIdAsync(Guid id);
        Task<List<Testimonial>> GetAllAsync();
        Task<(List<Testimonial> Items, int TotalCount)> GetAllPagedAsync(int page, int pageSize);
        Task<List<Testimonial>> GetVisibleAsync();
        Task<(List<Testimonial> Items, int TotalCount)> GetVisiblePagedAsync(int page, int pageSize);
        Task<Testimonial> AddAsync(Testimonial testimonial);
        Task UpdateAsync(Testimonial testimonial);
        Task DeleteAsync(Testimonial testimonial);
        Task<int> GetMaxOrderAsync();
    }
}
