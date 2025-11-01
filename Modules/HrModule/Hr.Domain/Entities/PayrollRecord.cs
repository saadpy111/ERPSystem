using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hr.Domain.Enums;

namespace Hr.Domain.Entities
{
    public class PayrollRecord : BaseEntity
    {
        [Key]
        public int PayrollId { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [Required]
        public int PeriodYear { get; set; }
        
        [Required]
        public int PeriodMonth { get; set; }

        public decimal BaseSalary { get; set; }
        public decimal TotalAllowances { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal NetSalary { get; set; }

        public PayrollStatus Status { get; set; } = PayrollStatus.Draft;

        public ICollection<PayrollComponent> Components { get; set; } = new List<PayrollComponent>();
    }
}
