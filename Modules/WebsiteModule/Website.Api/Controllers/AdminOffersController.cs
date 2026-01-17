using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Multitenancy;
using Website.Application.Features.OfferFeatures.Commands.CreateOffer;
using Website.Application.Features.OfferFeatures.Queries.GetAllOffers;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Admin endpoints for managing offers/discounts.
    /// </summary>
    [ApiController]
    [Route("api/website/admin/offers")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class AdminOffersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public AdminOffersController(IMediator mediator, IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        /// <summary>
        /// Get all offers.
        /// </summary>
        [HttpGet]
        [HasPermission(WebsitePermissions.OffersView)]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive = null)
        {
            var response = await _mediator.Send(new GetAllOffersQueryRequest { IsActive = isActive });
            return Ok(response.Offers);
        }

        /// <summary>
        /// Get an offer by ID.
        /// </summary>
        [HttpGet("{id}")]
        [HasPermission(WebsitePermissions.OffersView)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var repo = _unitOfWork.Repository<Offer>();
            var offer = await repo.GetByIdAsync(id, o => o.OfferProducts);
            if (offer == null) return NotFound();
            return Ok(offer);
        }

        /// <summary>
        /// Create a new offer.
        /// </summary>
        [HttpPost]
        [HasPermission(WebsitePermissions.OffersManage)]
        public async Task<IActionResult> Create([FromBody] CreateOfferCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success) return BadRequest(response.Message);
            return Ok(new { OfferId = response.OfferId });
        }

        /// <summary>
        /// Update an offer.
        /// </summary>
        [HttpPut("{id}")]
        [HasPermission(WebsitePermissions.OffersManage)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOfferRequest request)
        {
            var repo = _unitOfWork.Repository<Offer>();
            var offer = await repo.GetByIdAsync(id);
            if (offer == null) return NotFound();

            if (request.Name != null) offer.Name = request.Name;
            if (request.Description != null) offer.Description = request.Description;
            if (request.DiscountType.HasValue) offer.DiscountType = request.DiscountType.Value;
            if (request.DiscountValue.HasValue) offer.DiscountValue = request.DiscountValue.Value;
            if (request.StartDate.HasValue) offer.StartDate = request.StartDate.Value;
            if (request.EndDate.HasValue) offer.EndDate = request.EndDate.Value;
            if (request.IsActive.HasValue) offer.IsActive = request.IsActive.Value;
            offer.UpdatedAt = DateTime.UtcNow;

            repo.Update(offer);
            await _unitOfWork.SaveChangesAsync();
            return Ok(offer);
        }

        /// <summary>
        /// Delete an offer.
        /// </summary>
        [HttpDelete("{id}")]
        [HasPermission(WebsitePermissions.OffersManage)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var repo = _unitOfWork.Repository<Offer>();
            var offer = await repo.GetByIdAsync(id);
            if (offer == null) return NotFound();

            repo.Remove(offer);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Add a product to an offer.
        /// </summary>
        [HttpPost("{id}/products")]
        [HasPermission(WebsitePermissions.OffersManage)]
        public async Task<IActionResult> AddProduct(Guid id, [FromBody] AddProductToOfferRequest request)
        {
            var offerRepo = _unitOfWork.Repository<Offer>();
            var productRepo = _unitOfWork.Repository<WebsiteProduct>();
            var offerProductRepo = _unitOfWork.Repository<OfferProduct>();

            var offer = await offerRepo.GetByIdAsync(id);
            if (offer == null) return NotFound("Offer not found.");

            var product = await productRepo.GetByIdAsync(request.ProductId);
            if (product == null) return NotFound("Product not found.");

            var existing = await offerProductRepo.GetFirstAsync(op => op.OfferId == id && op.ProductId == request.ProductId);
            if (existing != null) return BadRequest("Product is already in this offer.");

            var offerProduct = new OfferProduct
            {
                OfferId = id,
                ProductId = request.ProductId,
                TenantId = _tenantProvider.GetTenantId() ?? string.Empty
            };

            await offerProductRepo.AddAsync(offerProduct);
            await _unitOfWork.SaveChangesAsync();
            return Ok(offerProduct);
        }

        /// <summary>
        /// Remove a product from an offer.
        /// </summary>
        [HttpDelete("{id}/products/{productId}")]
        [HasPermission(WebsitePermissions.OffersManage)]
        public async Task<IActionResult> RemoveProduct(Guid id, Guid productId)
        {
            var repo = _unitOfWork.Repository<OfferProduct>();
            var offerProduct = await repo.GetFirstAsync(op => op.OfferId == id && op.ProductId == productId);
            if (offerProduct == null) return NotFound();

            repo.Remove(offerProduct);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }

    // Request DTOs
    public record UpdateOfferRequest(
        string? Name,
        string? Description,
        DiscountType? DiscountType,
        decimal? DiscountValue,
        DateTime? StartDate,
        DateTime? EndDate,
        bool? IsActive
    );

    public record AddProductToOfferRequest(Guid ProductId);
}
