using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Hr.Persistence.Repositories
{
    public class RecruitmentStageRepository : IRecruitmentStageRepository
    {
        private readonly HrDbContext _context;

        public RecruitmentStageRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<RecruitmentStage> AddAsync(RecruitmentStage recruitmentStage)
        {
            await _context.RecruitmentStages.AddAsync(recruitmentStage);
            return recruitmentStage;
        }

        public async Task<RecruitmentStage?> GetByIdAsync(int id)
        {
            return await _context.RecruitmentStages
                .FirstOrDefaultAsync(rs => rs.StageId == id);
        }

        public async Task<IEnumerable<RecruitmentStage>> GetAllAsync()
        {
            return await _context.RecruitmentStages
                .OrderBy(rs => rs.SequenceOrder)
                .ToListAsync();
        }

        public void Update(RecruitmentStage recruitmentStage)
        {
            _context.RecruitmentStages.Update(recruitmentStage);
        }

        public void Delete(RecruitmentStage recruitmentStage)
        {
            _context.RecruitmentStages.Remove(recruitmentStage);
        }
    }
}
