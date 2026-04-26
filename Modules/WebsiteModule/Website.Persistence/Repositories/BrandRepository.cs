using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly WebsiteDbContext _context;

        public BrandRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<Brand?> GetByIdAsync(Guid id)
        {
            return await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> AddAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
            return brand;
        }

        public Task UpdateAsync(Brand brand)
        {
            _context.Entry(brand).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Brand brand)
        {
            _context.Brands.Remove(brand);
            return Task.CompletedTask;
        }
    }
}
