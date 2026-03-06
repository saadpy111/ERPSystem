using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Services
{
    public interface IOfferEligibilityService
    {
        /// <summary>
        /// Builds a dictionary mapping ProductId to all applicable active offers
        /// including Global, Product-scoped, and Category-scoped offers.
        /// </summary>
        /// <param name="products">List of products to check eligibility for.</param>
        /// <param name="tenantId">The current tenant identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Dictionary where Key is ProductId and Value is a list of applicable offers.</returns>
        Task<Dictionary<Guid, List<Offer>>> BuildProductOffersLookup(
            List<WebsiteProduct> products,
            string tenantId,
            CancellationToken cancellationToken = default);
    }

    public class OfferEligibilityService : IOfferEligibilityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OfferEligibilityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Dictionary<Guid, List<Offer>>> BuildProductOffersLookup(
            List<WebsiteProduct> products,
            string tenantId,
            CancellationToken cancellationToken = default)
        {
            if (products == null || !products.Any())
                return new Dictionary<Guid, List<Offer>>();

            var offerRepo = _unitOfWork.Repository<Offer>();
            var now = DateTime.UtcNow;

            // 1. Load active offers for the tenant including join tables
            var activeOffers = await offerRepo.GetAllAsync(
                o => o.TenantId == tenantId &&
                     o.IsActive &&
                     o.StartDate <= now &&
                     o.EndDate >= now,
                o => o.OfferProducts,
                o => o.OfferCategories);

            var lookup = new Dictionary<Guid, List<Offer>>();

            // 2. Pre-group offers by scope 
            var globalOffers = activeOffers
                .Where(o => o.ScopeType == OfferScopeType.AllProducts)
                .ToList();

            var productScopedOffers = activeOffers
                .Where(o => o.ScopeType == OfferScopeType.Product)
                .ToList();

            var categoryScopedOffers = activeOffers
                .Where(o => o.ScopeType == OfferScopeType.Category)
                .ToList();

            // 3. Match offers to each product
            foreach (var product in products)
            {
                var applicable = new List<Offer>();

                // a) Global offers apply to everyone
                applicable.AddRange(globalOffers);

                // b) Product-specific offers
                applicable.AddRange(productScopedOffers
                    .Where(o => o.OfferProducts.Any(op => op.ProductId == product.Id)));

                // c) Category-specific offers
                applicable.AddRange(categoryScopedOffers
                    .Where(o => o.OfferCategories.Any(oc => oc.CategoryId == product.CategoryId)));

                if (applicable.Any())
                {
                    lookup[product.Id] = applicable;
                }
            }

            return lookup;
        }
    }
}
