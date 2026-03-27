using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Features.PurchaseRequisitionFeatures.Commands;
using Procurement.Application.Features.PurchaseRequisitionFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Constants.Permissions;

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
        [Authorize(Permissions = ProcurementPermissions.PurchaseRequisitionsCreate)]
        public async Task<IActionResult> CreatePurchaseRequisition([FromBody] CreatePurchaseRequisitionCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpPut("{id:guid}")]
        [Authorize(Permissions = ProcurementPermissions.PurchaseRequisitionsEdit)]
        public async Task<IActionResult> UpdatePurchaseRequisition(Guid id, [FromBody] UpdatePurchaseRequisitionCommandRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpDelete("{id:guid}")]
        [Authorize(Permissions = ProcurementPermissions.PurchaseRequisitionsDelete)]
        public async Task<IActionResult> DeletePurchaseRequisition(Guid id)
        {
            var request = new DeletePurchaseRequisitionCommandRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpGet("{id:guid}")]
        [Authorize(Permissions = ProcurementPermissions.PurchaseRequisitionsView)]
        public async Task<IActionResult> GetPurchaseRequisitionById(Guid id)
        {
            var request = new GetPurchaseRequisitionByIdQueryRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return NotFound(response);
                
            return Ok(response);
        }
        
        [HttpGet]
        [Authorize(Permissions = ProcurementPermissions.PurchaseRequisitionsView)]
        public async Task<IActionResult> GetAllPurchaseRequisitions()
        {
            var request = new GetAllPurchaseRequisitionsQueryRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}