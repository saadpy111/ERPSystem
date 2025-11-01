using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.UpdatePayrollRecord
{
    public class UpdatePayrollRecordRequest : IRequest<UpdatePayrollRecordResponse>
    {
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public PayrollStatus Status { get; set; }
    }
}
