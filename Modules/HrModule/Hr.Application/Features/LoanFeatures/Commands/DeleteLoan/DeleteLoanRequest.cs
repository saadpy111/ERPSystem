using MediatR;

namespace Hr.Application.Features.LoanFeatures.DeleteLoan
{
    public class DeleteLoanRequest : IRequest<DeleteLoanResponse>
    {
        public int Id { get; set; }
    }
}
