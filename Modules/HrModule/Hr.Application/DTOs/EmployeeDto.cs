namespace Hr.Application.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Status { get; set; } = string.Empty;
        
        // New fields
        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public int? ProbationPeriod { get; set; }
        
        // Manager information
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        
        // Attachments
        public ICollection<HrAttachmentDto> Attachments { get; set; } = new List<HrAttachmentDto>();
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
