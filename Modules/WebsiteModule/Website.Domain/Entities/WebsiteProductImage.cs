using System;
using System.ComponentModel.DataAnnotations;

namespace Website.Domain.Entities
{
    public class WebsiteProductImage : BaseEntity
    {
        [MaxLength(300)]
        public string ImagePath { get; set; } = string.Empty;
        [MaxLength(300)]

        public string? AltText { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }

        public Guid WebsiteProductId { get; set; }
        public WebsiteProduct WebsiteProduct { get; set; } = null!;
    }
}
