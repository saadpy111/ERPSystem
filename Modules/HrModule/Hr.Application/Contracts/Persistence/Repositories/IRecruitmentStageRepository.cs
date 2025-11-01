using Hr.Domain.Entities;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IRecruitmentStageRepository
    {
        Task<RecruitmentStage> AddAsync(RecruitmentStage recruitmentStage);
        Task<RecruitmentStage?> GetByIdAsync(int id);
        Task<IEnumerable<RecruitmentStage>> GetAllAsync();
        void Update(RecruitmentStage recruitmentStage);
        void Delete(RecruitmentStage recruitmentStage);
    }
}
