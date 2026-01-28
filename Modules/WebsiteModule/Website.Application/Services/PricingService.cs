using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Services
{
    public interface IPricingService
    {
        /// <summary>
        /// Calculate the best applicable offer for a product from a pre-filtered list.
        /// </summary>
        /// <param name="unitPrice">Unit price of the product</param>
        /// <param name="quantity">Quantity in cart</param>
        /// <param name="applicableOffers">Pre-filtered offers applicable to this product</param>
        OfferCalculationResult CalculateBestOffer(decimal unitPrice, int quantity, List<Offer> applicableOffers);
    }

    public class PricingService : IPricingService
    {
        public OfferCalculationResult CalculateBestOffer(decimal unitPrice, int quantity, List<Offer> applicableOffers)
        {
            var originalPrice = unitPrice * quantity;

            var result = new OfferCalculationResult
            {
                OriginalPrice = originalPrice,
                DiscountAmount = 0,
                FinalPrice = originalPrice,
                AppliedOfferName = null
            };

            // No offers applicable
            if (!applicableOffers.Any())
            {
                return result;
            }

            // Calculate discount for each offer and pick the best one
            Offer? bestOffer = null;
            decimal bestDiscount = 0;

            foreach (var offer in applicableOffers)
            {
                var discount = CalculateDiscount(originalPrice, offer.DiscountType, offer.DiscountValue);

                if (discount > bestDiscount)
                {
                    bestDiscount = discount;
                    bestOffer = offer;
                }
            }

            if (bestOffer != null && bestDiscount > 0)
            {
                result.DiscountAmount = bestDiscount;
                result.FinalPrice = originalPrice - bestDiscount;
                result.AppliedOfferName = bestOffer.Name;
            }

            return result;
        }

        private decimal CalculateDiscount(decimal originalPrice, DiscountType discountType, decimal discountValue)
        {
            return discountType switch
            {
                DiscountType.Percentage => originalPrice * (discountValue / 100m),
                DiscountType.Fixed => Math.Min(discountValue, originalPrice), // Don't exceed original price
                _ => 0
            };
        }
    }

    /// <summary>
    /// Result of offer calculation for a cart item.
    /// </summary>
    public class OfferCalculationResult
    {
        public decimal OriginalPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }
        public string? AppliedOfferName { get; set; }
    }
}
