using MediatR;

namespace Hr.Application.Features.LoanInstallmentFeatures.DeleteLoanInstallment
{
    public class DeleteLoanInstallmentRequest : IRequest<DeleteLoanInstallmentResponse>
    {
        public int InstallmentId { get; set; }
    }
}
