using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Entities
{
    public class EmployeeReport : BaseEntity
    {
        public int EmployeeReportId { get; set; }

        // Main Data
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;

        public decimal Salary { get; set; }
        public string ContractType { get; set; } = string.Empty;

        public string ManagerName { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

       
    }
}
