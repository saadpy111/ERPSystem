using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.StorefrontFeatures.Services
{
    public interface IProductPricingService
    {
        ProductPricingResult CalculateBestPrice(decimal originalPrice, List<Offer> applicableOffers);
    }

    public class ProductPricingService : IProductPricingService
    {
        public ProductPricingResult CalculateBestPrice(decimal originalPrice, List<Offer> applicableOffers)
        {
            var result = new ProductPricingResult
            {
                OriginalPrice = originalPrice,
                FinalPrice = originalPrice,
                DiscountAmount = 0,
                AppliedOfferName = null,
                HasOffer = false
            };

            if (applicableOffers == null || !applicableOffers.Any())
            {
                return result;
            }

            decimal bestFinalPrice = originalPrice;
            Offer? bestOffer = null;

            foreach (var offer in applicableOffers)
            {
                decimal discountAmount = 0;
                
                if (offer.DiscountType == DiscountType.Percentage)
                {
                    discountAmount = originalPrice * (offer.DiscountValue / 100m);
                }
                else if (offer.DiscountType == DiscountType.Fixed)
                {
                    discountAmount = Math.Min(offer.DiscountValue, originalPrice);
                }

                decimal potentialFinalPrice = originalPrice - discountAmount;
                if (potentialFinalPrice < 0) potentialFinalPrice = 0;

                // Maximize discount (minimize final price)
                if (potentialFinalPrice < bestFinalPrice)
                {
                    bestFinalPrice = potentialFinalPrice;
                    bestOffer = offer;
                }
            }

            if (bestOffer != null && bestFinalPrice < originalPrice)
            {
                result.FinalPrice = bestFinalPrice;
                result.DiscountAmount = originalPrice - bestFinalPrice;
                result.AppliedOfferName = bestOffer.Name;
                result.HasOffer = true;
            }

            return result;
        }
    }

    public class ProductPricingResult
    {
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? AppliedOfferName { get; set; }
        public bool HasOffer { get; set; }
    }
}
