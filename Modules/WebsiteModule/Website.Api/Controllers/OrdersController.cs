using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using System.Security.Claims;
using Website.Application.Features.OrderFeatures.Commands.CreateOrder;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Api.Controllers
{
    /// <summary>
    /// Order endpoints for checkout and order management.
    /// </summary>
    [ApiController]
    [Route("api/website/orders")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        /// <summary>
        /// Get all orders for the current user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = GetUserId();
            var orderRepo = _unitOfWork.Repository<Order>();
            var orders = await orderRepo.GetAllAsync(
                o => o.UserId == userId,
                o => o.Items);

            var result = orders.OrderByDescending(o => o.OrderDate).Select(o => new
            {
                o.Id,
                o.OrderNumber,
                Status = o.Status.ToString(),
                o.TotalAmount,
                ItemCount = o.Items.Count,
                o.OrderDate
            });

            return Ok(result);
        }

        /// <summary>
        /// Get order details by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var userId = GetUserId();
            var orderRepo = _unitOfWork.Repository<Order>();
            var order = await orderRepo.GetFirstAsync(
                o => o.Id == id && o.UserId == userId,
                asNoTracking: true,
                o => o.Items);

            if (order == null) return NotFound();
            return Ok(order);
        }

        /// <summary>
        /// Create an order from the cart (checkout).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var response = await _mediator.Send(new CreateOrderCommandRequest
            {
                UserId = GetUserId(),
                PaymentMethod = request.PaymentMethod,
                Street = request.ShippingAddress.Street,
                City = request.ShippingAddress.City,
                State = request.ShippingAddress.State,
                Country = request.ShippingAddress.Country,
                ZipCode = request.ShippingAddress.ZipCode,
                Notes = request.Notes
            });

            if (!response.Success) return BadRequest(response.Message);
            return Ok(new { response.OrderId, response.OrderNumber });
        }

        /// <summary>
        /// Admin: Get all orders for the tenant.
        /// </summary>
        [HttpGet("admin/all")]
        [HasPermission(WebsitePermissions.OrdersView)]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderStatus? status = null)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var orders = await orderRepo.GetAllAsync(
                status.HasValue ? o => o.Status == status : null,
                o => o.Items);

            var result = orders.OrderByDescending(o => o.OrderDate).Select(o => new
            {
                o.Id,
                o.OrderNumber,
                Status = o.Status.ToString(),
                o.TotalAmount,
                ItemCount = o.Items.Count,
                o.OrderDate
            });

            return Ok(result);
        }

        /// <summary>
        /// Admin: Update order status.
        /// </summary>
        [HttpPut("admin/{id}/status")]
        [HasPermission(WebsitePermissions.OrdersManage)]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var order = await orderRepo.GetByIdAsync(id);
            if (order == null) return NotFound();

            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;

            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();
            return Ok(order);
        }
    }

    // DTOs
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
