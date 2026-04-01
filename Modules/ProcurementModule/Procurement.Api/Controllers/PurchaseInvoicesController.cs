using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Features.PurchaseInvoiceFeatures.Commands;
using Procurement.Application.Features.PurchaseInvoiceFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Authorization;

namespace Procurement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "procurement")]
    public class PurchaseInvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public PurchaseInvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [HasPermission(ProcurementPermissions.PurchaseInvoicesCreate)]
        public async Task<IActionResult> CreatePurchaseInvoice([FromBody] CreatePurchaseInvoiceCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpPut("{id:guid}")]
        [HasPermission(ProcurementPermissions.PurchaseInvoicesEdit)]
        public async Task<IActionResult> UpdatePurchaseInvoice(Guid id, [FromBody] UpdatePurchaseInvoiceCommandRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpDelete("{id:guid}")]
        [HasPermission(ProcurementPermissions.PurchaseInvoicesDelete)]
        public async Task<IActionResult> DeletePurchaseInvoice(Guid id)
        {
            var request = new DeletePurchaseInvoiceCommandRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpGet("{id:guid}")]
        [HasPermission(ProcurementPermissions.PurchaseInvoicesView)]
        public async Task<IActionResult> GetPurchaseInvoiceById(Guid id)
        {
            var request = new GetPurchaseInvoiceByIdQueryRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return NotFound(response);
                
            return Ok(response);
        }
        
        [HttpGet]
        [HasPermission(ProcurementPermissions.PurchaseInvoicesView)]
        public async Task<IActionResult> GetAllPurchaseInvoices()
        {
            var request = new GetAllPurchaseInvoicesQueryRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        
        [HttpGet("by-purchase-order/{purchaseOrderId:guid}")]
        [HasPermission(ProcurementPermissions.PurchaseInvoicesView)]
        public async Task<IActionResult> GetPurchaseInvoicesByPurchaseOrderId(Guid purchaseOrderId)
        {
            var request = new GetPurchaseInvoicesByPurchaseOrderIdQueryRequest { PurchaseOrderId = purchaseOrderId };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
