using System.ComponentModel.DataAnnotations;

namespace Hr.Domain.Entities
{
    public class RecruitmentStage : BaseEntity
    {
        [Key]
        public int StageId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public int SequenceOrder { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
