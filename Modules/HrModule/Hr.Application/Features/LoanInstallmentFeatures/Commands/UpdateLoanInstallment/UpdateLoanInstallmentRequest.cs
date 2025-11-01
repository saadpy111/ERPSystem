using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.LoanInstallmentFeatures.UpdateLoanInstallment
{
    public class UpdateLoanInstallmentRequest : IRequest<UpdateLoanInstallmentResponse>
    {
        public int InstallmentId { get; set; }
        public int LoanId { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public InstallmentStatus Status { get; set; }
    }
}
