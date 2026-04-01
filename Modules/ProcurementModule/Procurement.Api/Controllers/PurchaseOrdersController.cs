using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Features.PurchaseOrderFeatures.Commands;
using Procurement.Application.Features.PurchaseOrderFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Authorization;

namespace Procurement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "procurement")]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public PurchaseOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [HasPermission(ProcurementPermissions.PurchaseOrdersCreate)]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpPut("{id:guid}")]
        [HasPermission(ProcurementPermissions.PurchaseOrdersEdit)]
        public async Task<IActionResult> UpdatePurchaseOrder(Guid id, [FromBody] UpdatePurchaseOrderCommandRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpDelete("{id:guid}")]
        [HasPermission(ProcurementPermissions.PurchaseOrdersDelete)]
        public async Task<IActionResult> DeletePurchaseOrder(Guid id)
        {
            var request = new DeletePurchaseOrderCommandRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpGet("{id:guid}")]
        [HasPermission(ProcurementPermissions.PurchaseOrdersView)]
        public async Task<IActionResult> GetPurchaseOrderById(Guid id)
        {
            var request = new GetPurchaseOrderByIdQueryRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return NotFound(response);
                
            return Ok(response);
        }
        
        [HttpGet]
        [HasPermission(ProcurementPermissions.PurchaseOrdersView)]
        public async Task<IActionResult> GetAllPurchaseOrders()
        {
            var request = new GetAllPurchaseOrdersQueryRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
