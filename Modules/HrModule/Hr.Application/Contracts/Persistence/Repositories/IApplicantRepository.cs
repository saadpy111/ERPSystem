using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IApplicantRepository
    {
        Task<Applicant> AddAsync(Applicant applicant);
        Task<Applicant?> GetByIdAsync(int id);
        Task<IEnumerable<Applicant>> GetAllAsync();
        Task<IEnumerable<Applicant>> GetByJobIdAsync(int jobId);
        Task<PagedResult<Applicant>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, int? jobId = null, int? currentStageId = null, string? status = null, string? orderBy = null, bool isDescending = false);
        void Update(Applicant applicant);
        void Delete(Applicant applicant);
    }
}