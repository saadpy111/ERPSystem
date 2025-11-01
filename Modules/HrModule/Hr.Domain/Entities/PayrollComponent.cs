using System.ComponentModel.DataAnnotations;
using Hr.Domain.Enums;

namespace Hr.Domain.Entities
{
    public class PayrollComponent : BaseEntity
    {
        [Key]
        public int ComponentId { get; set; }

        [Required]
        public int PayrollRecordId { get; set; }
        public PayrollRecord PayrollRecord { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public PayrollComponentType ComponentType { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
