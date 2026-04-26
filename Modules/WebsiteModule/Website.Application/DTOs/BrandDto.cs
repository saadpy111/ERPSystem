using System;

namespace Website.Application.DTOs
{
    public class BrandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
