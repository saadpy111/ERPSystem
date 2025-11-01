using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.GetPayrollRecordById
{
    public class GetPayrollRecordByIdRequest : IRequest<GetPayrollRecordByIdResponse>
    {
        public int Id { get; set; }
    }
}
