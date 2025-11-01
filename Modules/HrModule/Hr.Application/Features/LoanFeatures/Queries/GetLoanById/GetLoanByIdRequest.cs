using MediatR;

namespace Hr.Application.Features.LoanFeatures.GetLoanById
{
    public class GetLoanByIdRequest : IRequest<GetLoanByIdResponse>
    {
        public int Id { get; set; }
    }
}
