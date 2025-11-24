using Report.Application.Contracts.Persistence.Repositories;
using Report.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Persistence.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly ReportDbContext _context;

        public ReportsRepository(ReportDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report.Domain.Entities.Report>> GetAllAsync()
        {
            return await _context.Reports
                .Include(r => r.Fields)
                .Include(r => r.Parameters)
                .Include(r => r.Filters)
                .Include(r => r.Groups)
                .Include(r => r.Sortings)
                .ToListAsync();
        }

        public async Task<Report.Domain.Entities.Report?> GetByIdAsync(int id)
        {
            return await _context.Reports
                .Include(r => r.Fields)
                .Include(r => r.Parameters)
                .Include(r => r.Filters)
                .Include(r => r.Groups)
                .Include(r => r.Sortings)
                .FirstOrDefaultAsync(r => r.ReportId == id);
        }
    }
}