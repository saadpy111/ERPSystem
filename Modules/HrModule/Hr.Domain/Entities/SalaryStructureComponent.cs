using Hr.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Domain.Entities
{
    public class SalaryStructureComponent : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SalaryStructureId { get; set; }
        public SalaryStructure SalaryStructure { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; 

        [Required]
        public PayrollComponentType Type { get; set; } 

        public decimal? FixedAmount { get; set; } 
        public decimal? Percentage { get; set; }   
    }

}
