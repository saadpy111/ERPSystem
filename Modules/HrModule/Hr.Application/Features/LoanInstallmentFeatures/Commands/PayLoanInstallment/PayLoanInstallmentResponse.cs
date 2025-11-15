namespace Hr.Application.Features.LoanInstallmentFeatures.PayLoanInstallment
{
    public class PayLoanInstallmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int InstallmentId { get; set; }
    }
}