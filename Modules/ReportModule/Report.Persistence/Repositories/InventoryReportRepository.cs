using Report.Application.Contracts.Persistence.Repositories;
using Report.Domain.Entities;
using Report.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.Repositories
{
    public class InventoryReportRepository : IInventoryReportRepository
    {
        private readonly ReportDbContext _context;

        public InventoryReportRepository(ReportDbContext context)
        {
            _context = context;
        }

        public async Task<InventoryReport> AddAsync(InventoryReport inventoryReport)
        {
            await _context.InventoryReports.AddAsync(inventoryReport);
            return inventoryReport;
        }

        public async Task<IEnumerable<InventoryReport>> GetAllAsync()
        {
            return await _context.InventoryReports.ToListAsync();
        }
    }
}