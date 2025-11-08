using Hr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IHrAttachmentRepository
    {
        Task<HrAttachment> AddAsync(HrAttachment attachment);
        Task<HrAttachment?> GetByIdAsync(int id);
        Task<IEnumerable<HrAttachment>> GetAllAsync();
        Task<IEnumerable<HrAttachment>> GetByEntityAsync(string entityType, int entityId);
        void Update(HrAttachment attachment);
        void Delete(HrAttachment attachment);
    }
}