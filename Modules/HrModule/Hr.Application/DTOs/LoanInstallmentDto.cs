using Hr.Domain.Enums;

namespace Hr.Application.DTOs
{
    public class LoanInstallmentDto
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
