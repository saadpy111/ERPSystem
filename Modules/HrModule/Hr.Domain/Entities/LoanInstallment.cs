using System;
using System.ComponentModel.DataAnnotations;
using Hr.Domain.Enums;

namespace Hr.Domain.Entities
{
    public class LoanInstallment : BaseEntity
    {
        [Key]
        public int InstallmentId { get; set; }

        [Required]
        public int LoanId { get; set; }
        public Loan Loan { get; set; } = null!;

        public DateTime DueDate { get; set; }

        [Required]
        public decimal AmountDue { get; set; }

        public DateTime? PaymentDate { get; set; }

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        public InstallmentStatus Status { get; set; } = InstallmentStatus.Pending;
    }
}
