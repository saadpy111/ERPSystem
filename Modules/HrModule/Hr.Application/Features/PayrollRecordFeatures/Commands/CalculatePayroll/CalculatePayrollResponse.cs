using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Features.PayrollRecordFeatures.Commands.RecalculatePayroll
{
    public class CalculatePayrollResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public decimal TotalAllowances { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal NetSalary { get; set; }
    }
}