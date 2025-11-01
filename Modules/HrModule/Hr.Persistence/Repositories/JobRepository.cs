using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Entities;
using Hr.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Hr.Persistence.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly HrDbContext _context;

        public JobRepository(HrDbContext context)
        {
            _context = context;
        }

        public async Task<Job> AddAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
            return job;
        }

        public async Task<Job?> GetByIdAsync(int id)
        {
            return await _context.Jobs
                .Include(j => j.Department)
                .Include(j => j.Applicants)
                .FirstOrDefaultAsync(j => j.JobId == id);
        }

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _context.Jobs
                .Include(j => j.Department)
                .ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.Jobs
                .Where(j => j.DepartmentId == departmentId)
                .Include(j => j.Department)
                .ToListAsync();
        }

        public void Update(Job job)
        {
            _context.Jobs.Update(job);
        }

        public void Delete(Job job)
        {
            _context.Jobs.Remove(job);
        }
    }
}
