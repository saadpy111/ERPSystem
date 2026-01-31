using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using Website.Application.Features.WebsiteProductFeatures.Commands.PublishProduct;
using Website.Application.Features.WebsiteProductFeatures.Commands.UnpublishProduct;
using Website.Application.Features.WebsiteProductFeatures.Commands.UpdateProductPrice;
using Website.Application.Features.WebsiteProductFeatures.Commands.UpdateProductImages;
using Website.Application.Features.WebsiteProductFeatures.Queries.GetAllProducts;
using Website.Application.Features.WebsiteProductFeatures.Queries.GetInventoryProducts;
using Website.Application.Features.WebsiteProductFeatures.Queries.GetProductById;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Admin endpoints for managing website products.
    /// Products are published from Inventory and managed here.
    /// </summary>
    [ApiController]
    [Route("api/website/admin")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class AdminProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Inventory Products (Read-Only from Inventory)

        /// <summary>
        /// List products from Inventory module for publishing selection.
        /// Read-only, queries Inventory directly.
        /// </summary>
        [HttpGet("inventory-products")]
        [HasPermission(WebsitePermissions.ProductsPublish)]
        public async Task<IActionResult> GetInventoryProducts([FromQuery] GetInventoryProductsQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        #endregion

        #region Website Products (Published)

        /// <summary>
        /// Get all published website products with optional filters.
        /// </summary>
        [HttpGet("products")]
        [HasPermission(WebsitePermissions.ProductsView)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response.Result);
        }

        /// <summary>
        /// Get a published product by ID.
        /// </summary>
        [HttpGet("products/{id}")]
        [HasPermission(WebsitePermissions.ProductsView)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetProductByIdQueryRequest { Id = id });
            if (response.Product == null) return NotFound();
            return Ok(response.Product);
        }

        /// <summary>
        /// Publish a product from Inventory to the website.
        /// Accepts only IDs - fetches product data from Inventory.
        /// </summary>
        [HttpPost("products/publish")]
        [HasPermission(WebsitePermissions.ProductsPublish)]
        public async Task<IActionResult> Publish([FromBody] PublishProductCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success) return BadRequest(response.Message);
            return Ok(new { response.ProductId, response.Message });
        }

        /// <summary>
        /// Unpublish a product from the website.
        /// Hides from storefront but preserves snapshot for re-publish.
        /// </summary>
        [HttpPost("products/{id}/unpublish")]
        [HasPermission(WebsitePermissions.ProductsPublish)]
        public async Task<IActionResult> Unpublish(Guid id)
        {
            var response = await _mediator.Send(new UnpublishProductCommandRequest { WebsiteProductId = id });
            if (!response.Success) return NotFound(response.Message);
            return Ok(new { response.Message });
        }

        /// <summary>
        /// Update product price.
        /// </summary>
        [HttpPut("products/{id}/price")]
        [HasPermission(WebsitePermissions.ProductsEdit)]
        public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] decimal newPrice)
        {
            var response = await _mediator.Send(new UpdateProductPriceCommandRequest
            {
                ProductId = id,
                NewPrice = newPrice
            });
            if (!response.Success) return NotFound(response.Message);
            return Ok();
        }

        /// <summary>
        /// Update product images.
        /// </summary>
        [HttpPut("products/{id}/images")]
        [Consumes("multipart/form-data")]
        [HasPermission(WebsitePermissions.ProductsEdit)]
        public async Task<IActionResult> UpdateImages(Guid id, [FromForm] UpdateWebsiteProductImagesCommandRequest request)
        {
            request.ProductId = id;
            var response = await _mediator.Send(request);
            if (!response.Success) return BadRequest(response.Message);
            return Ok(response);
        }

        #endregion
    }
}
