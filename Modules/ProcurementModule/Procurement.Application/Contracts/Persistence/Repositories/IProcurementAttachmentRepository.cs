using Procurement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Procurement.Application.Contracts.Persistence.Repositories
{
    public interface IProcurementAttachmentRepository
    {
        Task<ProcurementAttachment> AddAsync(ProcurementAttachment attachment);
        Task<ProcurementAttachment> GetByIdAsync(Guid id);
        Task<IEnumerable<ProcurementAttachment>> GetAllAsync();
        Task<IEnumerable<ProcurementAttachment>> GetAllByEntityAsync(string entityType, Guid entityId);
        void Update(ProcurementAttachment attachment);
        void Delete(ProcurementAttachment attachment);
    }
}