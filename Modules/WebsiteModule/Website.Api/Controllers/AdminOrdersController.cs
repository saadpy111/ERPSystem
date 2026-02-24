using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using Website.Application.DTOs;
using Website.Application.Features.OrderFeatures.Commands.UpdateOrderStatus;
using Website.Application.Features.OrderFeatures.Queries.GetAdminDashboardStats;
using Website.Application.Features.OrderFeatures.Queries.GetAdminOrderDetails;
using Website.Application.Features.OrderFeatures.Queries.GetAdminOrdersPaged;
using Website.Domain.Enums;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/admin/orders")]
    [ApiExplorerSettings(GroupName = "Website")]
    [Authorize]
    public class AdminOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(WebsitePermissions.OrdersView)]
        public async Task<IActionResult> GetAdminOrders([FromQuery] AdminOrderFilter filter)
        {
            var query = new GetAdminOrdersPagedQuery 
            { 
                Filter = filter 
            };
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var response = await _mediator.Send(new GetAdminDashboardStatsQuery());
            return Ok(response);
        }
        [HttpGet("{id}")]
        [HasPermission(WebsitePermissions.OrdersView)]
        public async Task<IActionResult> GetAdminOrder(Guid id)
        {
            var query = new GetAdminOrderDetailsQuery 
            { 
                OrderId = id 
            };
            var response = await _mediator.Send(query);

            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPut("{id}/status")]
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
    }

    public record UpdateOrderStatusRequest(OrderStatus Status);
}
