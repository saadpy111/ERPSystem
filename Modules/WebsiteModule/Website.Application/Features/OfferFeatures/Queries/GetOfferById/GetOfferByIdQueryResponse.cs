using Website.Domain.Enums;

namespace Website.Application.Features.OfferFeatures.Queries.GetOfferById
{
    public class GetOfferByIdQueryResponse
    {
        public OfferDetailDto? Offer { get; set; }
    }

    public class OfferDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<OfferProductDto> Products { get; set; } = new();
    }

    public class OfferProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}
