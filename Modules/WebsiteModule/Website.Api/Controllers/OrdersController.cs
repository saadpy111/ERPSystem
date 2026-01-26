using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using System.Security.Claims;
using Website.Application.Features.OrderFeatures.Commands.CreateOrder;
using Website.Application.Features.OrderFeatures.Commands.UpdateOrderStatus;
using Website.Application.Features.OrderFeatures.Queries.GetAllOrders;
using Website.Application.Features.OrderFeatures.Queries.GetOrderById;
using Website.Application.Features.OrderFeatures.Queries.GetUserOrders;
using Website.Domain.Enums;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Order endpoints for checkout and order management.
    /// Thin controller using CQRS pattern via MediatR.
    /// </summary>
    [ApiController]
    [Route("api/website/orders")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        #region Customer Endpoints

        /// <summary>
        /// Get all orders for the current user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetUserOrdersQueryRequest { UserId = GetUserId() });
            return Ok(response.Orders);
        }

        /// <summary>
        /// Get order details by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var response = await _mediator.Send(new GetOrderByIdQueryRequest 
            { 
                OrderId = id, 
                UserId = GetUserId() 
            });

            if (response.Order == null) return NotFound();
            return Ok(response.Order);
        }

        /// <summary>
        /// Create an order from the cart (checkout).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var command = new CreateOrderCommandRequest
            {
                UserId = GetUserId(),
                PaymentMethod = request.PaymentMethod,
                Street = request.ShippingAddress.Street,
                City = request.ShippingAddress.City,
                State = request.ShippingAddress.State,
                Country = request.ShippingAddress.Country,
                ZipCode = request.ShippingAddress.ZipCode,
                Notes = request.Notes
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return BadRequest(response.Message);
            
            return Ok(new { response.OrderId, response.OrderNumber });
        }

        #endregion

        #region Admin Endpoints

        /// <summary>
        /// Admin: Get all orders for the tenant.
        /// </summary>
        [HttpGet("admin/all")]
        [HasPermission(WebsitePermissions.OrdersView)]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderStatus? status = null)
        {
            var response = await _mediator.Send(new GetAllOrdersQueryRequest { Status = status });
            return Ok(response.Orders);
        }

        /// <summary>
        /// Admin: Update order status.
        /// </summary>
        [HttpPut("admin/{id}/status")]
        [HasPermission(WebsitePermissions.OrdersManage)]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
        {
            var command = new UpdateOrderStatusCommandRequest
            {
                OrderId = id,
                Status = request.Status
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return NotFound(response.Message);
            
            return Ok();
        }

        #endregion
    }

    // Request DTOs (used only for HTTP binding, not domain logic)
    public record CheckoutRequest(
        PaymentMethod PaymentMethod,
        ShippingAddressDto ShippingAddress,
        string? Notes
    );

    public record UpdateOrderStatusRequest(OrderStatus Status);

    public class ShippingAddressDto
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}
