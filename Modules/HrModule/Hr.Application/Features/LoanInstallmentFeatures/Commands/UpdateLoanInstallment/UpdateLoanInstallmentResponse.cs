using Hr.Application.DTOs;

namespace Hr.Application.Features.LoanInstallmentFeatures.UpdateLoanInstallment
{
    public class UpdateLoanInstallmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public LoanInstallmentDto? LoanInstallment { get; set; }
    }
}
