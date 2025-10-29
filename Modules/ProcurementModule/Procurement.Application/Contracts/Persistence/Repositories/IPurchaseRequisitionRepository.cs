using Procurement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Procurement.Application.Contracts.Persistence.Repositories
{
    public interface IPurchaseRequisitionRepository
    {
        Task<PurchaseRequisition> AddAsync(PurchaseRequisition requisition);
        Task<PurchaseRequisition> GetByIdAsync(Guid id);
        Task<IEnumerable<PurchaseRequisition>> GetAllAsync();
        void Update(PurchaseRequisition requisition);
        void Delete(PurchaseRequisition requisition);
    }
}