using Report.Application.Contracts.Persistence.Repositories;
using Report.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Persistence.Repositories
{
    public class ReportDataSourcesRepository : IReportDataSourcesRepository
    {
        private readonly ReportDbContext _context;

        public ReportDataSourcesRepository(ReportDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report.Domain.Entities.ReportDataSource>> GetAllAsync()
        {
            return await _context.ReportDataSources
                .ToListAsync();
        }

        public async Task<Report.Domain.Entities.ReportDataSource?> GetByIdAsync(int id)
        {
            return await _context.ReportDataSources
                .FirstOrDefaultAsync(ds => ds.ReportDataSourceId == id);
        }
    }
}