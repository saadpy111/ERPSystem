using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Features.PurchaseOrderItemFeatures.Commands;
using Procurement.Application.Features.PurchaseOrderItemFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Constants.Permissions;

namespace Procurement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "procurement")]
    public class PurchaseOrderItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public PurchaseOrderItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [Authorize(Permissions = ProcurementPermissions.PurchaseOrderItemsCreate)]
        public async Task<IActionResult> CreatePurchaseOrderItem([FromBody] CreatePurchaseOrderItemCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpPut("{id:guid}")]
        [Authorize(Permissions = ProcurementPermissions.PurchaseOrderItemsEdit)]
        public async Task<IActionResult> UpdatePurchaseOrderItem(Guid id, [FromBody] UpdatePurchaseOrderItemCommandRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpDelete("{id:guid}")]
        [Authorize(Permissions = ProcurementPermissions.PurchaseOrderItemsDelete)]
        public async Task<IActionResult> DeletePurchaseOrderItem(Guid id)
        {
            var request = new DeletePurchaseOrderItemCommandRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpGet("{id:guid}")]
        [Authorize(Permissions = ProcurementPermissions.PurchaseOrderItemsView)]
        public async Task<IActionResult> GetPurchaseOrderItemById(Guid id)
        {
            var request = new GetPurchaseOrderItemByIdQueryRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return NotFound(response);
                
            return Ok(response);
        }
        
        [HttpGet]
        [Authorize(Permissions = ProcurementPermissions.PurchaseOrderItemsView)]
        public async Task<IActionResult> GetAllPurchaseOrderItems()
        {
            var request = new GetAllPurchaseOrderItemsQueryRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        
        [HttpGet("by-purchase-order/{purchaseOrderId:guid}")]
        [Authorize(Permissions = ProcurementPermissions.PurchaseOrderItemsView)]
        public async Task<IActionResult> GetPurchaseOrderItemsByPurchaseOrderId(Guid purchaseOrderId)
        {
            var request = new GetPurchaseOrderItemsByPurchaseOrderIdQueryRequest { PurchaseOrderId = purchaseOrderId };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}