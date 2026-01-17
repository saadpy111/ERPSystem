using MediatR;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.PublishProduct
{
    /// <summary>
    /// Publish product request - only accepts IDs, fetches data from Inventory.
    /// </summary>
    public class PublishProductCommandRequest : IRequest<PublishProductCommandResponse>
    {
        /// <summary>
        /// The Inventory product ID to publish.
        /// </summary>
        public Guid InventoryProductId { get; set; }

        /// <summary>
        /// The Website category ID to assign the product to.
        /// </summary>
        public Guid WebsiteCategoryId { get; set; }

        /// <summary>
        /// Display order for storefront sorting.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;
    }
}
