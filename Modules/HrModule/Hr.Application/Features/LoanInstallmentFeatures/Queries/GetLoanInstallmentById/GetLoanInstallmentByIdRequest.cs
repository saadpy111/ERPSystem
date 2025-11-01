using MediatR;

namespace Hr.Application.Features.LoanInstallmentFeatures.GetLoanInstallmentById
{
    public class GetLoanInstallmentByIdRequest : IRequest<GetLoanInstallmentByIdResponse>
    {
        public int Id { get; set; }
    }
}
