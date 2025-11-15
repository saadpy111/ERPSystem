using Hr.Application.Features.EmployeeContractFeatures.Commands.CreateEmployeeContract;
using Hr.Application.Features.EmployeeContractFeatures.Commands.DeleteAttachmentContract;
using Hr.Application.Features.EmployeeContractFeatures.Commands.DeleteEmployeeContract;
using Hr.Application.Features.EmployeeContractFeatures.Commands.UpdateEmployeeContract;
using Hr.Application.Features.EmployeeContractFeatures.Commands.UploadAttachmentContract;
using Hr.Application.Features.EmployeeContractFeatures.Queries.GetAllEmployeeContracts;
using Hr.Application.Features.EmployeeContractFeatures.Queries.GetEmployeeContractById;
using Hr.Application.Features.EmployeeContractFeatures.Queries.GetEmployeeContractsByEmployeeId;
using Hr.Application.Features.EmployeeContractFeatures.Queries.GetEmployeeContractsPaged;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class EmployeeContractsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeContractsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateEmployeeContractRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllEmployeeContractsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetEmployeeContractsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetEmployeeContractByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.EmployeeContract == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var query = new GetEmployeeContractsByEmployeeIdRequest { EmployeeId = employeeId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateEmployeeContractRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteEmployeeContractRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("{id}/attachments")]
        public async Task<IActionResult> UploadAttachment(int id, [FromForm] UploadAttachmentContractRequest request)
        {
            request.EmployeeContractId = id;
            var result = await _mediator.Send(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("attachments/{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(int attachmentId)
        {
            var request = new DeleteAttachmentContractRequest { AttachmentId = attachmentId };
            var result = await _mediator.Send(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Enum Endpoints

        [HttpGet("enums/contract-types")]
        public IActionResult GetContractTypes()
        {
            var contractTypes = Enum.GetValues(typeof(Hr.Domain.Enums.ContractType))
                .Cast<Hr.Domain.Enums.ContractType>()
                .Select(ct => new { Value = (int)ct, Name = ct.ToString() });
            return Ok(contractTypes);
        }
    }
}