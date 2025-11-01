using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.GetPayrollComponentById
{
    public class GetPayrollComponentByIdRequest : IRequest<GetPayrollComponentByIdResponse>
    {
        public int Id { get; set; }
    }
}
