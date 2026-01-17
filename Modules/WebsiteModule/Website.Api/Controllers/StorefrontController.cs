using MediatR;
using Microsoft.AspNetCore.Mvc;
using Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontCategories;
using Website.Application.Features.StorefrontFeatures.Queries.GetStorefrontProducts;
using Website.Application.Features.CollectionFeatures.Queries.GetAllCollections;
using Website.Application.Contracts.Persistence.Repositories;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Public storefront endpoints for browsing products and categories.
    /// No authentication required for browsing.
    /// </summary>
    [ApiController]
    [Route("api/website/storefront")]
    [ApiExplorerSettings(GroupName = "Website")]
    public class StorefrontController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWebsiteProductRepository _productRepository;

        public StorefrontController(IMediator mediator, IWebsiteProductRepository productRepository)
        {
            _mediator = mediator;
            _productRepository = productRepository;
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
            var product = await _productRepository.GetProductWithCategoryAsync(id);
            if (product == null || !product.IsPublished) return NotFound();

            return Ok(new
            {
                product.Id,
                Name = product.NameSnapshot,
                ImageUrl = product.ImageUrlSnapshot,
                product.CategoryId,
                CategoryName = product.CategoryNameSnapshot,
                product.Price,
                product.IsAvailable
            });
        }

        /// <summary>
        /// Get all active categories as a tree structure.
        /// </summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _mediator.Send(new GetStorefrontCategoriesQueryRequest());
            return Ok(response.Categories);
        }

        /// <summary>
        /// Get all active collections.
        /// </summary>
        [HttpGet("collections")]
        public async Task<IActionResult> GetCollections()
        {
            var response = await _mediator.Send(new GetAllCollectionsQueryRequest { IsActive = true });
            return Ok(response.Collections);
        }
    }
}
