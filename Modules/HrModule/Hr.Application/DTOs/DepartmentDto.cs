namespace Hr.Application.DTOs
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
