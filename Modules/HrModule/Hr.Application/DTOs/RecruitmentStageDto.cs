namespace Hr.Application.DTOs
{
    public class RecruitmentStageDto
    {
        public int StageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SequenceOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
