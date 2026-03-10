using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website.Application.Features.OrderFeatures.Commands.CreateOrder;
using Website.Application.Features.OrderFeatures.Queries.GetUserOrdersPaged;
using Website.Application.Features.OrderFeatures.Queries.GetUserOrderDetails;
using Website.Domain.Enums;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class UserOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetUserOrdersPagedQuery 
            { 
                UserId = GetUserId(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var query = new GetUserOrderDetailsQuery 
            { 
                OrderId = id, 
                UserId = GetUserId() 
            };
            var response = await _mediator.Send(query);

            if (response == null) return NotFound();
            return Ok(response);
        }

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
                Notes = request.Notes,
                CouponCode = request.CouponCode
            };

            var response = await _mediator.Send(command);
            if (!response.Success) return BadRequest(response.Message);
            
            return Ok(new { response.OrderId, response.OrderNumber });
        }

        [HttpGet("order-status")]
        [AllowAnonymous]
        public IActionResult GetOrderStatus()
        {
            var values = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(e => new
                {
                    Value = (int)e,
                    Name = e.ToString()
                })
                .ToList();

            return Ok(values);
        }

        [HttpGet("payment-method")]
        [AllowAnonymous]
        public IActionResult GetPaymentMethod()
        {
            var values = Enum.GetValues(typeof(PaymentMethod))
                .Cast<PaymentMethod>()
                .Select(e => new
                {
                    Value = (int)e,
                    Name = e.ToString()
                })
                .ToList();

            return Ok(values);
        }
    }

    public record CheckoutRequest(
        PaymentMethod PaymentMethod,
        ShippingAddressDto ShippingAddress,
        string? Notes,
        string? CouponCode

    );

    public class ShippingAddressDto
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}
