using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Hr.Persistence.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly HrDbContext _context;

        public ApplicantRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<Applicant> AddAsync(Applicant applicant)
        {
            await _context.Applicants.AddAsync(applicant);
            return applicant;
        }

        public async Task<Applicant?> GetByIdAsync(int id)
        {
            return await _context.Applicants
                .Include(a => a.AppliedJob)
                .Include(a => a.CurrentStage)
                .FirstOrDefaultAsync(a => a.ApplicantId == id);
        }

        public async Task<IEnumerable<Applicant>> GetAllAsync()
        {
            return await _context.Applicants
                .Include(a => a.AppliedJob)
                .Include(a => a.CurrentStage)
                .ToListAsync();
        }

        public async Task<IEnumerable<Applicant>> GetByJobIdAsync(int jobId)
        {
            return await _context.Applicants
                .Where(a => a.JobId == jobId)
                .Include(a => a.AppliedJob)
                .Include(a => a.CurrentStage)
                .ToListAsync();
        }

        public void Update(Applicant applicant)
        {
            _context.Applicants.Update(applicant);
        }

        public void Delete(Applicant applicant)
        {
            _context.Applicants.Remove(applicant);
        }
    }
}
