using System;

namespace Website.Application.DTOs
{
    public class FavoriteProductDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal? FinalPrice { get; set; }
        public string? MainImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public double? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public DateTime AddedToFavoritesAt { get; set; }
    }

    public class FavoriteStatusDto
    {
        public Guid ProductId { get; set; }
        public bool IsFavorite { get; set; }
    }

    public class FavoriteCountDto
    {
        public int Count { get; set; }
    }

    public class AddFavoriteResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class RemoveFavoriteResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
