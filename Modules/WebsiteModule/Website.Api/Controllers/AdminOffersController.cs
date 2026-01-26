using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using Website.Application.Features.OfferFeatures.Commands.AddProductToOffer;
using Website.Application.Features.OfferFeatures.Commands.CreateOffer;
using Website.Application.Features.OfferFeatures.Commands.DeleteOffer;
using Website.Application.Features.OfferFeatures.Commands.RemoveProductFromOffer;
using Website.Application.Features.OfferFeatures.Commands.UpdateOffer;
using Website.Application.Features.OfferFeatures.Queries.GetAllOffers;
using Website.Application.Features.OfferFeatures.Queries.GetOfferById;
using Website.Domain.Enums;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Admin endpoints for managing offers/discounts.
    /// Thin controller using CQRS pattern via MediatR.
    /// </summary>
    [ApiController]
    [Route("api/website/admin/offers")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class AdminOffersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminOffersController(IMediator mediator)
        {
            _mediator = mediator;
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
            var response = await _mediator.Send(new GetOfferByIdQueryRequest { Id = id });
            if (response.Offer == null) return NotFound();
            return Ok(response.Offer);
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
            var command = new UpdateOfferCommandRequest
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = request.IsActive
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return NotFound(response.Message);
            return Ok();
        }

        /// <summary>
        /// Delete an offer.
        /// </summary>
        [HttpDelete("{id}")]
        [HasPermission(WebsitePermissions.OffersManage)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteOfferCommandRequest { Id = id });
            if (!response.Success) return NotFound(response.Message);
            return NoContent();
        }

        /// <summary>
        /// Add a product to an offer.
        /// </summary>
        [HttpPost("{id}/products")]
        [HasPermission(WebsitePermissions.OffersManage)]
        public async Task<IActionResult> AddProduct(Guid id, [FromBody] AddProductToOfferRequest request)
        {
            var command = new AddProductToOfferCommandRequest
            {
                OfferId = id,
                ProductId = request.ProductId
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return BadRequest(response.Message);
            return Ok();
        }

        /// <summary>
        /// Remove a product from an offer.
        /// </summary>
        [HttpDelete("{id}/products/{productId}")]
        [HasPermission(WebsitePermissions.OffersManage)]
        public async Task<IActionResult> RemoveProduct(Guid id, Guid productId)
        {
            var command = new RemoveProductFromOfferCommandRequest
            {
                OfferId = id,
                ProductId = productId
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return NotFound(response.Message);
            return NoContent();
        }
    }

    // Request DTOs (used only for HTTP binding, not domain logic)
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
