using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence
{
    public interface IBrandRepository
    {
        Task<Brand?> GetByIdAsync(Guid id);
        Task<List<Brand>> GetAllAsync();
        Task<Brand> AddAsync(Brand brand);
        Task UpdateAsync(Brand brand);
        Task DeleteAsync(Brand brand);
    }
}
