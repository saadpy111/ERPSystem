using Report.Application.Contracts.Persistence.Repositories;
using Report.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Persistence.Repositories
{
    public class ReportParametersRepository : IReportParametersRepository
    {
        private readonly ReportDbContext _context;

        public ReportParametersRepository(ReportDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report.Domain.Entities.ReportParameter>> GetAllAsync()
        {
            return await _context.ReportParameters
                .Include(p => p.Report)
                .ToListAsync();
        }

        public async Task<Report.Domain.Entities.ReportParameter?> GetByIdAsync(int id)
        {
            return await _context.ReportParameters
                .Include(p => p.Report)
                .FirstOrDefaultAsync(p => p.ReportParameterId == id);
        }
    }
}