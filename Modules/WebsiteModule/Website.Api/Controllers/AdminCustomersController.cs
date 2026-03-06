using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;
using Website.Application.DTOs;
using Website.Application.Features.CustomerFeatures.Queries.GetCustomerDetails;
using Website.Application.Features.CustomerFeatures.Queries.GetCustomersPaged;

namespace Website.Api.Controllers
{
    [ApiController]
    [Route("api/admin/customers")]
    [ApiExplorerSettings(GroupName = "Website")]
 //   [Authorize]
    public class AdminCustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminCustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
       // [HasPermission(WebsitePermissions.CustomersView)]
        public async Task<IActionResult> GetCustomers([FromQuery] CustomerFilter filter)
        {
            var response = await _mediator.Send(new GetCustomersPagedQuery { Filter = filter });
            return Ok(response);
        }

        [HttpGet("{id}")]
     //   [HasPermission(WebsitePermissions.CustomersView)]
        public async Task<IActionResult> GetCustomerDetails(Guid id, [FromQuery] int ordersPage = 1, [FromQuery] int ordersPageSize = 10)
        {
            var response = await _mediator.Send(new GetCustomerDetailsQuery 
            { 
                CustomerId = id,
                OrdersPage = ordersPage,
                OrdersPageSize = ordersPageSize
            });

            if (response == null) return NotFound();

            return Ok(response);
        }
    }
}
