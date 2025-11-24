using Report.Application.Contracts.Persistence.Repositories;
using Report.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Persistence.Repositories
{
    public class ReportSortingsRepository : IReportSortingsRepository
    {
        private readonly ReportDbContext _context;

        public ReportSortingsRepository(ReportDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report.Domain.Entities.ReportSorting>> GetAllAsync()
        {
            return await _context.ReportSortings
                .Include(s => s.Report)
                .ToListAsync();
        }

        public async Task<Report.Domain.Entities.ReportSorting?> GetByIdAsync(int id)
        {
            return await _context.ReportSortings
                .Include(s => s.Report)
                .FirstOrDefaultAsync(s => s.ReportSortingId == id);
        }
    }
}