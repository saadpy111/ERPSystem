using System;

namespace Website.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
