using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IApplicantEducationRepository
    {
        Task<ApplicantEducation> AddAsync(ApplicantEducation applicantEducation);
        Task<ApplicantEducation?> GetByIdAsync(int id);
        Task<IEnumerable<ApplicantEducation>> GetAllAsync();
        Task<PagedResult<ApplicantEducation>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false);
        void Update(ApplicantEducation applicantEducation);
        void Delete(ApplicantEducation applicantEducation);
        Task<IEnumerable<ApplicantEducation>> GetByApplicantIdAsync(int applicantId);
        Task RemoveEducations(int applicantId);
        Task AddEducations(int applicantId, IEnumerable<ApplicantEducation> educations);
    }
}