namespace Hr.Application.DTOs
{
    public class ApplicantEducationDto
    {
        public int Id { get; set; } = 0;
        public string DegreeName { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public int? GraduationYear { get; set; }
        public string? Institute { get; set; }
        public string? Grade { get; set; }
        public int ApplicantId { get; set; }
    }
}