using Website.Domain.Enums;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontOfferById
{
    public class GetStorefrontOfferByIdQueryResponse
    {
        public StorefrontOfferDetailDto? Offer { get; set; }
    }

    public class StorefrontOfferDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public List<StorefrontOfferProductDto> Products { get; set; } = new();
    }

    public class StorefrontOfferProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal OriginalPrice { get; set; }
    }
}
