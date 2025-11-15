using MediatR;
using System;

namespace Hr.Application.Features.LoanInstallmentFeatures.PayLoanInstallment
{
    public class PayLoanInstallmentRequest : IRequest<PayLoanInstallmentResponse>
    {
        public int InstallmentId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string? PaymentMethod { get; set; }
    }
}