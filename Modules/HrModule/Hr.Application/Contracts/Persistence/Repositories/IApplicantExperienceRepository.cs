using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IApplicantExperienceRepository
    {
        Task<ApplicantExperience> AddAsync(ApplicantExperience applicantExperience);
        Task<ApplicantExperience?> GetByIdAsync(int id);
        Task<IEnumerable<ApplicantExperience>> GetAllAsync();
        Task<PagedResult<ApplicantExperience>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false);
        void Update(ApplicantExperience applicantExperience);
        void Delete(ApplicantExperience applicantExperience);
        Task<IEnumerable<ApplicantExperience>> GetByApplicantIdAsync(int applicantId);
        Task RemoveExperiences(int applicantId);
        Task AddExperiences(int applicantId, IEnumerable<ApplicantExperience> experiences);
    }
}