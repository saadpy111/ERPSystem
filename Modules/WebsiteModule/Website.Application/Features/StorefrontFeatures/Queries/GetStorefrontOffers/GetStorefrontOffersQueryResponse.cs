using Website.Application.Pagination;
using Website.Domain.Enums;

namespace Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontOffers
{
    public class GetStorefrontOffersQueryResponse
    {
        public PagedResult<StorefrontOfferDto>? Result { get; set; }
    }

    public class StorefrontOfferDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
