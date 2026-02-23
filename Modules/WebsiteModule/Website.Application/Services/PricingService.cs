using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Services
{
    public interface IPricingService
    {
        /// <summary>
        /// Selects the winning offer from a pre-filtered list and computes
        /// the final price for a cart line.
        ///
        /// Conflict-resolution algorithm (deterministic):
        ///   1. Pick the offer with the lowest <see cref="Offer.Priority"/> value.
        ///   2. If two or more offers share the same priority, pick the one
        ///      that produces the highest monetary discount for this line.
        ///   3. If that is still tied (identical discount amounts), the first
        ///      offer in the pre-sorted enumeration wins (stable sort).
        /// </summary>
        /// <param name="unitPrice">Unit price of the product.</param>
        /// <param name="quantity">Quantity ordered.</param>
        /// <param name="applicableOffers">
        ///   Pre-filtered list of active offers that apply to this product
        ///   (merged from product-based and category-based offers by the caller).
        /// </param>
        OfferCalculationResult CalculateBestOffer(decimal unitPrice, int quantity, List<Offer> applicableOffers);
    }

    public class PricingService : IPricingService
    {
        public OfferCalculationResult CalculateBestOffer(
            decimal unitPrice,
            int quantity,
            List<Offer> applicableOffers)
        {
            var lineTotal = unitPrice * quantity;

            var result = new OfferCalculationResult
            {
                OriginalPrice    = lineTotal,
                DiscountAmount   = 0,
                FinalPrice       = lineTotal,
                AppliedOfferName = null
            };

            if (applicableOffers == null || applicableOffers.Count == 0)
                return result;

            // ── Step 1: find the highest-priority tier (lowest Priority value) ────────
            int bestPriority = applicableOffers.Min(o => o.Priority);

            var topTierOffers = applicableOffers
                .Where(o => o.Priority == bestPriority)
                .ToList();

            // ── Step 2: within the top tier, pick the one with the highest discount ──
            Offer? winningOffer   = null;
            decimal bestDiscount  = 0;

            foreach (var offer in topTierOffers)
            {
                decimal discount = ComputeDiscount(lineTotal, offer.DiscountType, offer.DiscountValue);

                // Strictly greater — first occurrence wins ties (stable resolution)
                if (discount > bestDiscount)
                {
                    bestDiscount  = discount;
                    winningOffer  = offer;
                }
            }

            // ── Step 3: apply the winning offer if it produces a real discount ────────
            if (winningOffer != null && bestDiscount > 0)
            {
                result.DiscountAmount   = bestDiscount;
                result.FinalPrice       = lineTotal - bestDiscount;
                result.AppliedOfferName = winningOffer.Name;
            }

            return result;
        }

        // ── Private helper ────────────────────────────────────────────────────────────

        private static decimal ComputeDiscount(
            decimal lineTotal,
            DiscountType discountType,
            decimal discountValue)
        {
            decimal rawDiscount = discountType switch
            {
                DiscountType.Percentage => lineTotal * (Math.Max(0, discountValue) / 100m),
                DiscountType.Fixed      => Math.Max(0, discountValue),
                _                      => 0m
            };

            // Harden: Never exceed line total
            return Math.Min(rawDiscount, lineTotal);
        }
    }

    /// <summary>Result of offer calculation for a single cart line.</summary>
    public class OfferCalculationResult
    {
        public decimal OriginalPrice    { get; set; }
        public decimal DiscountAmount   { get; set; }
        public decimal FinalPrice       { get; set; }
        public string? AppliedOfferName { get; set; }
    }
}
