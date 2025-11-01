using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.CreatePayrollRecord
{
    public class CreatePayrollRecordRequest : IRequest<CreatePayrollRecordResponse>
    {
        public int EmployeeId { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalDeductions { get; set; }
    }
}
