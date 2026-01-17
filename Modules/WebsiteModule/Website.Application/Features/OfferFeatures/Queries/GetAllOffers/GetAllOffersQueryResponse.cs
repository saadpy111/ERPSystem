using Website.Domain.Enums;

namespace Website.Application.Features.OfferFeatures.Queries.GetAllOffers
{
    public class GetAllOffersQueryResponse
    {
        public List<OfferDto> Offers { get; set; } = new();
    }

    public class OfferDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
    }
}
