using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using Website.Application.Features.CollectionFeatures.Commands.AddProductToCollection;
using Website.Application.Features.CollectionFeatures.Commands.CreateCollection;
using Website.Application.Features.CollectionFeatures.Commands.DeleteCollection;
using Website.Application.Features.CollectionFeatures.Commands.RemoveProductFromCollection;
using Website.Application.Features.CollectionFeatures.Commands.UpdateCollection;
using Website.Application.Features.CollectionFeatures.Queries.GetAllCollections;
using Website.Application.Features.CollectionFeatures.Queries.GetCollectionById;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Admin endpoints for managing product collections.
    /// Thin controller using CQRS pattern via MediatR.
    /// </summary>
    [ApiController]
    [Route("api/website/admin/collections")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class AdminCollectionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminCollectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all collections.
        /// </summary>
        [HttpGet]
        [HasPermission(WebsitePermissions.CollectionsView)]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive = null)
        {
            var response = await _mediator.Send(new GetAllCollectionsQueryRequest { IsActive = isActive });
            return Ok(response.Collections);
        }

        /// <summary>
        /// Get a collection by ID.
        /// </summary>
        [HttpGet("{id}")]
        [HasPermission(WebsitePermissions.CollectionsView)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediator.Send(new GetCollectionByIdQueryRequest { Id = id });
            if (response.Collection == null) return NotFound();
            return Ok(response.Collection);
        }

        /// <summary>
        /// Create a new collection.
        /// </summary>
        [HttpPost]
        [HasPermission(WebsitePermissions.CollectionsManage)]
        public async Task<IActionResult> Create([FromBody] CreateCollectionCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success) return BadRequest(response.Message);
            return Ok(new { CollectionId = response.CollectionId });
        }

        /// <summary>
        /// Update a collection.
        /// </summary>
        [HttpPut("{id}")]
        [HasPermission(WebsitePermissions.CollectionsManage)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCollectionRequest request)
        {
            var command = new UpdateCollectionCommandRequest
            {
                Id = id,
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                IsActive = request.IsActive,
                DisplayOrder = request.DisplayOrder
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return NotFound(response.Message);
            return Ok();
        }

        /// <summary>
        /// Delete a collection.
        /// </summary>
        [HttpDelete("{id}")]
        [HasPermission(WebsitePermissions.CollectionsManage)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteCollectionCommandRequest { Id = id });
            if (!response.Success) return NotFound(response.Message);
            return NoContent();
        }

        /// <summary>
        /// Add a product to a collection.
        /// </summary>
        [HttpPost("{id}/products")]
        [HasPermission(WebsitePermissions.CollectionsManage)]
        public async Task<IActionResult> AddProduct(Guid id, [FromBody] AddProductRequest request)
        {
            var command = new AddProductToCollectionCommandRequest
            {
                CollectionId = id,
                ProductId = request.ProductId,
                DisplayOrder = request.DisplayOrder
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return BadRequest(response.Message);
            return Ok();
        }

        /// <summary>
        /// Remove a product from a collection.
        /// </summary>
        [HttpDelete("{id}/products/{productId}")]
        [HasPermission(WebsitePermissions.CollectionsManage)]
        public async Task<IActionResult> RemoveProduct(Guid id, Guid productId)
        {
            var command = new RemoveProductFromCollectionCommandRequest
            {
                CollectionId = id,
                ProductId = productId
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return NotFound(response.Message);
            return NoContent();
        }
    }

    // Request DTOs (used only for HTTP binding, not domain logic)
    public record UpdateCollectionRequest(
        string? Name,
        string? Slug,
        string? Description,
        string? ImageUrl,
        bool? IsActive,
        int? DisplayOrder
    );

    public record AddProductRequest(Guid ProductId, int DisplayOrder = 0);
}
