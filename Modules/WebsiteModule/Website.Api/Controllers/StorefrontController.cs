using MediatR;
using Microsoft.AspNetCore.Mvc;
using Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCategories;
using Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProducts;
using Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProductById;
using Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCollections;
using Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontOffers;
using Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontOfferById;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Public storefront endpoints for browsing products, categories, collections, and offers.
    /// No authentication required for browsing.
    /// </summary>
    [ApiController]
    [Route("api/website/storefront")]
    [ApiExplorerSettings(GroupName = "Website")]
    public class StorefrontController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StorefrontController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all published products with optional filters.
        /// </summary>
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts([FromQuery] GetStorefrontProductsQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response.Result);
        }

        /// <summary>
        /// Get product details by ID.
        /// </summary>
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var response = await _mediator.Send(new GetStorefrontProductByIdQueryRequest { Id = id });
            
            if (response.Product == null)
            {
                return NotFound();
            }

            return Ok(response.Product);
        }

        /// <summary>
        /// Get all active categories as a paginated tree structure.
        /// </summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery] GetStorefrontCategoriesQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response.Result);
        }

        /// <summary>
        /// Get all active collections (paginated).
        /// </summary>
        [HttpGet("collections")]
        public async Task<IActionResult> GetCollections([FromQuery] GetStorefrontCollectionsQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response.Result);
        }

        /// <summary>
        /// Get all active offers (paginated).
        /// </summary>
        [HttpGet("offers")]
        public async Task<IActionResult> GetOffers([FromQuery] GetStorefrontOffersQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response.Result);
        }

        /// <summary>
        /// Get offer details by ID.
        /// </summary>
        [HttpGet("offers/{id}")]
        public async Task<IActionResult> GetOffer(Guid id)
        {
            var response = await _mediator.Send(new GetStorefrontOfferByIdQueryRequest { Id = id });

            if (response.Offer == null)
            {
                return NotFound();
            }

            return Ok(response.Offer);
        }
    }
}
