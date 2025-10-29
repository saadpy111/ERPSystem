using Procurement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Procurement.Application.Contracts.Persistence.Repositories
{
    public interface IVendorRepository
    {
        Task<Vendor> AddAsync(Vendor vendor);
        Task<Vendor> GetByIdAsync(Guid id);
        Task<IEnumerable<Vendor>> GetAllAsync();
        Task<IEnumerable<Vendor>> GetActiveVendorsAsync();
        void Update(Vendor vendor);
        void Delete(Vendor vendor);
    }
}