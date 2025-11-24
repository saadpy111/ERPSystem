using Report.Application.Contracts.Persistence.Repositories;
using Report.Domain.Entities;
using Report.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Persistence.Repositories
{
    public class EmployeeReportRepository : IEmployeeReportRepository
    {
        private readonly ReportDbContext _context;

        public EmployeeReportRepository(ReportDbContext context)
        {
               _context = context;
        }
        public async Task<EmployeeReport> AddAsync(EmployeeReport employeeReport)
        {
            await _context.EmployeeReports.AddAsync(employeeReport);
            return employeeReport;
        }
    }
}
