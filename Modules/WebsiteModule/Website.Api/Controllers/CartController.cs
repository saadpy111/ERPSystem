using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website.Application.Features.CartFeatures.Commands.AddToCart;
using Website.Application.Features.CartFeatures.Queries.GetCart;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Shopping cart endpoints for authenticated users.
    /// </summary>
    [ApiController]
    [Route("api/website/cart")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
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
            var response = await _mediator.Send(new AddToCartCommandRequest
            {
                UserId = GetUserId(),
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });

            if (!response.Success) return BadRequest(response.Message);
            return await GetCart();
        }

        /// <summary>
        /// Update cart item quantity.
        /// </summary>
        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] UpdateCartItemRequest request)
        {
            var cartItemRepo = _unitOfWork.Repository<CartItem>();
            var item = await cartItemRepo.GetByIdAsync(itemId);
            if (item == null) return NotFound();

            if (request.Quantity <= 0)
            {
                cartItemRepo.Remove(item);
            }
            else
            {
                item.Quantity = request.Quantity;
                item.UpdatedAt = DateTime.UtcNow;
                cartItemRepo.Update(item);
            }

            await _unitOfWork.SaveChangesAsync();
            return await GetCart();
        }

        /// <summary>
        /// Remove an item from the cart.
        /// </summary>
        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            var cartItemRepo = _unitOfWork.Repository<CartItem>();
            var item = await cartItemRepo.GetByIdAsync(itemId);
            if (item == null) return NotFound();

            cartItemRepo.Remove(item);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Clear all items from the cart.
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            var cartRepo = _unitOfWork.Repository<Cart>();
            var cartItemRepo = _unitOfWork.Repository<CartItem>();

            var cart = await cartRepo.GetFirstAsync(c => c.UserId == userId && !c.IsCheckedOut, false, c => c.Items);
            if (cart != null)
            {
                cartItemRepo.RemoveRange(cart.Items);
                await _unitOfWork.SaveChangesAsync();
            }

            return NoContent();
        }
    }

    // DTOs
    public record AddToCartRequest(Guid ProductId, int Quantity = 1);
    public record UpdateCartItemRequest(int Quantity);
}
