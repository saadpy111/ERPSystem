using Hr.Application.DTOs;

namespace Hr.Application.Features.LoanInstallmentFeatures.CreateLoanInstallment
{
    public class CreateLoanInstallmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public LoanInstallmentDto? LoanInstallment { get; set; }
    }
}
