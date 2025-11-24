using Report.Application.Contracts.Persistence.Repositories;
using Report.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Persistence.Repositories
{
    public class ReportFiltersRepository : IReportFiltersRepository
    {
        private readonly ReportDbContext _context;

        public ReportFiltersRepository(ReportDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report.Domain.Entities.ReportFilter>> GetAllAsync()
        {
            return await _context.ReportFilters
                .Include(f => f.Report)
                .ToListAsync();
        }

        public async Task<Report.Domain.Entities.ReportFilter?> GetByIdAsync(int id)
        {
            return await _context.ReportFilters
                .Include(f => f.Report)
                .FirstOrDefaultAsync(f => f.ReportFilterId == id);
        }
    }
}