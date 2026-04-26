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
    public class TestimonialRepository : ITestimonialRepository
    {
        private readonly WebsiteDbContext _context;

        public TestimonialRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<Testimonial?> GetByIdAsync(Guid id)
        {
            return await _context.Testimonials.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Testimonial>> GetAllAsync()
        {
            return await _context.Testimonials
                .OrderBy(t => t.Order)
                .ToListAsync();
        }

        public async Task<(List<Testimonial> Items, int TotalCount)> GetAllPagedAsync(int page, int pageSize)
        {
            var query = _context.Testimonials.OrderBy(t => t.Order);
            
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }

        public async Task<List<Testimonial>> GetVisibleAsync()
        {
            return await _context.Testimonials
                .Where(t => t.IsVisible)
                .OrderBy(t => t.Order)
                .ToListAsync();
        }

        public async Task<(List<Testimonial> Items, int TotalCount)> GetVisiblePagedAsync(int page, int pageSize)
        {
            var query = _context.Testimonials.Where(t => t.IsVisible).OrderBy(t => t.Order);
            
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }

        public async Task<Testimonial> AddAsync(Testimonial testimonial)
        {
            await _context.Testimonials.AddAsync(testimonial);
            return testimonial;
        }

        public Task UpdateAsync(Testimonial testimonial)
        {
            _context.Testimonials.Update(testimonial);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Testimonial testimonial)
        {
            _context.Testimonials.Remove(testimonial);
            return Task.CompletedTask;
        }

        public async Task<int> GetMaxOrderAsync()
        {
            var maxOrder = await _context.Testimonials
                .Select(t => (int?)t.Order)
                .MaxAsync();
                
            return maxOrder ?? 0;
        }
    }
}
