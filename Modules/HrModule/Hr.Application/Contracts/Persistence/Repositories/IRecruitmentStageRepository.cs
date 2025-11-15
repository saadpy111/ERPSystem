using Hr.Application.Pagination;
using Hr.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IRecruitmentStageRepository
    {
        Task<RecruitmentStage> AddAsync(RecruitmentStage recruitmentStage);
        Task<RecruitmentStage?> GetByIdAsync(int id);
        Task<IEnumerable<RecruitmentStage>> GetAllAsync();
        Task<PagedResult<RecruitmentStage>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, string? orderBy = null, bool isDescending = false);
        void Update(RecruitmentStage recruitmentStage);
        void Delete(RecruitmentStage recruitmentStage);
    }
}