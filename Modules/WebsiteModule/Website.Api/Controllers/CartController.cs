using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website.Application.Features.CartFeatures.Commands.AddToCart;
using Website.Application.Features.CartFeatures.Commands.ClearCart;
using Website.Application.Features.CartFeatures.Commands.RemoveCartItem;
using Website.Application.Features.CartFeatures.Commands.UpdateCartItemQuantity;
using Website.Application.Features.CartFeatures.Queries.GetCart;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Shopping cart endpoints for authenticated users.
    /// Thin controller using CQRS pattern via MediatR.
    /// </summary>
    [ApiController]
    [Route("api/website/cart")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        /// <summary>
        /// Get the current user's cart.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var response = await _mediator.Send(new GetCartQueryRequest { UserId = GetUserId() });
            return Ok(response.Cart);
        }

        /// <summary>
        /// Add a product to the cart.
        /// </summary>
        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddToCartRequest request)
        {
            var command = new AddToCartCommandRequest
            {
                UserId = GetUserId(),
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return BadRequest(response.Message);
            
            return await GetCart();
        }

        /// <summary>
        /// Update cart item quantity.
        /// </summary>
        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] UpdateCartItemRequest request)
        {
            var command = new UpdateCartItemQuantityCommandRequest
            {
                ItemId = itemId,
                Quantity = request.Quantity
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return NotFound(response.Message);
            
            return await GetCart();
        }

        /// <summary>
        /// Remove an item from the cart.
        /// </summary>
        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            var response = await _mediator.Send(new RemoveCartItemCommandRequest { ItemId = itemId });
            if (!response.Success) return NotFound(response.Message);
            
            return NoContent();
        }

        /// <summary>
        /// Clear all items from the cart.
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var response = await _mediator.Send(new ClearCartCommandRequest { UserId = GetUserId() });
            return NoContent();
        }
    }

    // Request DTOs (used only for HTTP binding, not domain logic)
    public record AddToCartRequest(Guid ProductId, int Quantity = 1);
    public record UpdateCartItemRequest(int Quantity);
}
