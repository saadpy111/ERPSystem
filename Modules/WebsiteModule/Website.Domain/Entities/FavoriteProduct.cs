using System;

namespace Website.Domain.Entities
{
    public class FavoriteProduct : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public WebsiteProduct Product { get; set; } = null!;
    }
}
