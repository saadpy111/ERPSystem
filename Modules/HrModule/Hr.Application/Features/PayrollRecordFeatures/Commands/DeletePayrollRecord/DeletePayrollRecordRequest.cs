using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.DeletePayrollRecord
{
    public class DeletePayrollRecordRequest : IRequest<DeletePayrollRecordResponse>
    {
        public int PayrollId { get; set; }
    }
}
