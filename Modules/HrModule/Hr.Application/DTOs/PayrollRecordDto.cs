using Hr.Domain.Enums;
using System.Collections.Generic;

namespace Hr.Application.DTOs
{
    public class PayrollRecordDto
    {
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public PayrollStatus Status { get; set; }
        public ICollection<PayrollComponentDto> Components { get; set; } = new List<PayrollComponentDto>();
    }
}