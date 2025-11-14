using Hr.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Domain.Entities
{
    public class SalaryStructure : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime EffectiveDate { get; set; }

        public SalaryStructureType Type { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<SalaryStructureComponent> Components { get; set; } = new List<SalaryStructureComponent>();
    }
}