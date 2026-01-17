using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Multitenancy;
using Website.Application.Features.CollectionFeatures.Commands.CreateCollection;
using Website.Application.Features.CollectionFeatures.Queries.GetAllCollections;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Admin endpoints for managing product collections.
    /// </summary>
    [ApiController]
    [Route("api/website/admin/collections")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class AdminCollectionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public AdminCollectionsController(IMediator mediator, IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
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
            var repo = _unitOfWork.Repository<ProductCollection>();
            var collection = await repo.GetByIdAsync(id, c => c.Items);
            if (collection == null) return NotFound();
            return Ok(collection);
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
            var repo = _unitOfWork.Repository<ProductCollection>();
            var collection = await repo.GetByIdAsync(id);
            if (collection == null) return NotFound();

            if (request.Name != null) collection.Name = request.Name;
            if (request.Slug != null) collection.Slug = request.Slug;
            if (request.Description != null) collection.Description = request.Description;
            if (request.ImageUrl != null) collection.ImageUrl = request.ImageUrl;
            if (request.IsActive.HasValue) collection.IsActive = request.IsActive.Value;
            if (request.DisplayOrder.HasValue) collection.DisplayOrder = request.DisplayOrder.Value;
            collection.UpdatedAt = DateTime.UtcNow;

            repo.Update(collection);
            await _unitOfWork.SaveChangesAsync();
            return Ok(collection);
        }

        /// <summary>
        /// Delete a collection.
        /// </summary>
        [HttpDelete("{id}")]
        [HasPermission(WebsitePermissions.CollectionsManage)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var repo = _unitOfWork.Repository<ProductCollection>();
            var collection = await repo.GetByIdAsync(id);
            if (collection == null) return NotFound();

            repo.Remove(collection);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Add a product to a collection.
        /// </summary>
        [HttpPost("{id}/products")]
        [HasPermission(WebsitePermissions.CollectionsManage)]
        public async Task<IActionResult> AddProduct(Guid id, [FromBody] AddProductRequest request)
        {
            var collectionRepo = _unitOfWork.Repository<ProductCollection>();
            var productRepo = _unitOfWork.Repository<WebsiteProduct>();
            var itemRepo = _unitOfWork.Repository<ProductCollectionItem>();

            var collection = await collectionRepo.GetByIdAsync(id);
            if (collection == null) return NotFound("Collection not found.");

            var product = await productRepo.GetByIdAsync(request.ProductId);
            if (product == null) return NotFound("Product not found.");

            var existing = await itemRepo.GetFirstAsync(i => i.CollectionId == id && i.ProductId == request.ProductId);
            if (existing != null) return BadRequest("Product is already in this collection.");

            var item = new ProductCollectionItem
            {
                CollectionId = id,
                ProductId = request.ProductId,
                DisplayOrder = request.DisplayOrder,
                TenantId = _tenantProvider.GetTenantId() ?? string.Empty
            };

            await itemRepo.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return Ok(item);
        }

        /// <summary>
        /// Remove a product from a collection.
        /// </summary>
        [HttpDelete("{id}/products/{productId}")]
        [HasPermission(WebsitePermissions.CollectionsManage)]
        public async Task<IActionResult> RemoveProduct(Guid id, Guid productId)
        {
            var itemRepo = _unitOfWork.Repository<ProductCollectionItem>();
            var item = await itemRepo.GetFirstAsync(i => i.CollectionId == id && i.ProductId == productId);
            if (item == null) return NotFound();

            itemRepo.Remove(item);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }

    // Request DTOs
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
