using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Features.VendorFeatures.Commands;
using Procurement.Application.Features.VendorFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using SharedKernel.Constants.Permissions;
using SharedKernel.Authorization;

namespace Procurement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "procurement")]
    public class VendorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public VendorsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [HasPermission(ProcurementPermissions.VendorsCreate)]
        public async Task<IActionResult> CreateVendor([FromForm] CreateVendorCommandRequest request)
        {
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpPut("{id:guid}")]
        [HasPermission(ProcurementPermissions.VendorsEdit)]
        public async Task<IActionResult> UpdateVendor(Guid id, [FromForm] UpdateVendorCommandRequest request)
        {
            request.Vendor.Id = id;
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpDelete("{id:guid}")]
        [HasPermission(ProcurementPermissions.VendorsDelete)]
        public async Task<IActionResult> DeleteVendor(Guid id)
        {
            var request = new DeleteVendorCommandRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return BadRequest(response);
                
            return Ok(response);
        }
        
        [HttpGet("{id:guid}")]
        [HasPermission(ProcurementPermissions.VendorsView)]
        public async Task<IActionResult> GetVendorById(Guid id)
        {
            var request = new GetVendorByIdQueryRequest { Id = id };
            var response = await _mediator.Send(request);
            if (!response.Success)
                return NotFound(response);
                
            return Ok(response);
        }
        
        [HttpGet]
        [HasPermission(ProcurementPermissions.VendorsView)]
        public async Task<IActionResult> GetAllVendors()
        {
            var request = new GetAllVendorsQueryRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
