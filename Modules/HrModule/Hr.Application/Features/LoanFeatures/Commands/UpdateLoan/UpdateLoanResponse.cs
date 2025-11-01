using Hr.Application.DTOs;

namespace Hr.Application.Features.LoanFeatures.UpdateLoan
{
    public class UpdateLoanResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public LoanDto? Loan { get; set; }
    }
}
