using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.UpdatePayrollRecord
{
    public class UpdatePayrollRecordRequest : IRequest<UpdatePayrollRecordResponse>
    {
        public int PayrollId { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public PayrollStatus Status { get; set; }
    }
}
