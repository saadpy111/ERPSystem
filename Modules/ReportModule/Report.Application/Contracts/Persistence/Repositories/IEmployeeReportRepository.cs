using Report.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Contracts.Persistence.Repositories
{
    public interface IEmployeeReportRepository
    {
        Task<EmployeeReport> AddAsync(EmployeeReport employeeReport);
    }
}
