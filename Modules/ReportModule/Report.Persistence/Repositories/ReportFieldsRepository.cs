using Report.Application.Contracts.Persistence.Repositories;
using Report.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Persistence.Repositories
{
    public class ReportFieldsRepository : IReportFieldsRepository
    {
        private readonly ReportDbContext _context;

        public ReportFieldsRepository(ReportDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report.Domain.Entities.ReportField>> GetAllAsync()
        {
            return await _context.ReportFields
                .Include(f => f.Report)
                .ToListAsync();
        }

        public async Task<Report.Domain.Entities.ReportField?> GetByIdAsync(int id)
        {
            return await _context.ReportFields
                .Include(f => f.Report)
                .FirstOrDefaultAsync(f => f.ReportFieldId == id);
        }
    }
}