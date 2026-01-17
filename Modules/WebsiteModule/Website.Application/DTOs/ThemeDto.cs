using Website.Domain.ValueObjects;

namespace Website.Application.DTOs
{
    /// <summary>
    /// DTO for Theme entity.
    /// </summary>
    public class ThemeDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PreviewImage { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public ThemeConfig Config { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
