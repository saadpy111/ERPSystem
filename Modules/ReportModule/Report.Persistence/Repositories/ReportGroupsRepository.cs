using Report.Application.Contracts.Persistence.Repositories;
using Report.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Persistence.Repositories
{
    public class ReportGroupsRepository : IReportGroupsRepository
    {
        private readonly ReportDbContext _context;

        public ReportGroupsRepository(ReportDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report.Domain.Entities.ReportGroup>> GetAllAsync()
        {
            return await _context.ReportGroups
                .Include(g => g.Report)
                .ToListAsync();
        }

        public async Task<Report.Domain.Entities.ReportGroup?> GetByIdAsync(int id)
        {
            return await _context.ReportGroups
                .Include(g => g.Report)
                .FirstOrDefaultAsync(g => g.ReportGroupId == id);
        }
    }
}