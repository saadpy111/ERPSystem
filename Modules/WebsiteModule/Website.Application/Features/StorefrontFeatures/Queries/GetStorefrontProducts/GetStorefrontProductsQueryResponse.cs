using Website.Application.Pagination;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProducts
{
    public class GetStorefrontProductsQueryResponse
    {
        public PagedResult<StorefrontProductDto>? Result { get; set; }
    }

    public class StorefrontProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        
        /// <summary>
        /// Original price before any discounts (was Price).
        /// </summary>
        public decimal OriginalPrice { get; set; }
        
        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }
        public string? AppliedOfferName { get; set; }
        public bool HasOffer { get; set; }
        
        public bool IsAvailable { get; set; }
    }
}
