using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Features.PurchaseRequisitionFeatures.Commands;
using Procurement.Application.Features.PurchaseRequisitionFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Authorization;

namespace Procurement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "procurement")]
    public class PurchaseRequisitionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public PurchaseRequisitionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [HasPermission(ProcurementPermissions.PurchaseRequisitionsCreate)]
        public async Task<IActionResult> CreatePurchaseRequisition([FromBody] CreatePurchaseRequisitionCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpPut("{id:guid}")]
        [HasPermission(ProcurementPermissions.PurchaseRequisitionsEdit)]
        public async Task<IActionResult> UpdatePurchaseRequisition(Guid id, [FromBody] UpdatePurchaseRequisitionCommandRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpDelete("{id:guid}")]
        [HasPermission(ProcurementPermissions.PurchaseRequisitionsDelete)]
        public async Task<IActionResult> DeletePurchaseRequisition(Guid id)
        {
            var request = new DeletePurchaseRequisitionCommandRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpGet("{id:guid}")]
        [HasPermission(ProcurementPermissions.PurchaseRequisitionsView)]
        public async Task<IActionResult> GetPurchaseRequisitionById(Guid id)
        {
            var request = new GetPurchaseRequisitionByIdQueryRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return NotFound(response);
                
            return Ok(response);
        }
        
        [HttpGet]
        [HasPermission(ProcurementPermissions.PurchaseRequisitionsView)]
        public async Task<IActionResult> GetAllPurchaseRequisitions()
        {
            var request = new GetAllPurchaseRequisitionsQueryRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
