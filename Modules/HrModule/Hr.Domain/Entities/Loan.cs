using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hr.Domain.Enums;

namespace Hr.Domain.Entities
{
    public class Loan : BaseEntity
    {
        [Key]
        public int LoanId { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [Required]
        public decimal PrincipalAmount { get; set; }

        [Required]
        public decimal MonthlyInstallment { get; set; }

        [Required]
        public int TermMonths { get; set; }

        public DateTime StartDate { get; set; }

        public decimal RemainingBalance { get; set; }

        public LoanStatus Status { get; set; } = LoanStatus.Active;
        
        
        public string? Notes { get; set; }

        public ICollection<LoanInstallment> Installments { get; set; } = new List<LoanInstallment>();
    }
}