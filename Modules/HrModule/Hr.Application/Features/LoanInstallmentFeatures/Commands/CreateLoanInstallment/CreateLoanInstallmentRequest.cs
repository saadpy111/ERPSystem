using MediatR;
using Hr.Domain.Enums;

namespace Hr.Application.Features.LoanInstallmentFeatures.CreateLoanInstallment
{
    public class CreateLoanInstallmentRequest : IRequest<CreateLoanInstallmentResponse>
    {
        public int LoanId { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
