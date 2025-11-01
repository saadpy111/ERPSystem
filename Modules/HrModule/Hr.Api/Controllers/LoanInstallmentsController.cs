using Hr.Application.Features.LoanInstallmentFeatures.CreateLoanInstallment;
using Hr.Application.Features.LoanInstallmentFeatures.DeleteLoanInstallment;
using Hr.Application.Features.LoanInstallmentFeatures.GetAllLoanInstallments;
using Hr.Application.Features.LoanInstallmentFeatures.GetLoanInstallmentById;
using Hr.Application.Features.LoanInstallmentFeatures.GetLoanInstallmentsPaged;
using Hr.Application.Features.LoanInstallmentFeatures.UpdateLoanInstallment;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class LoanInstallmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoanInstallmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLoanInstallmentRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllLoanInstallmentsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetLoanInstallmentsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetLoanInstallmentByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.LoanInstallment == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLoanInstallmentRequest request)
        {
            if (id != request.InstallmentId)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteLoanInstallmentRequest { InstallmentId = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Enum Endpoints

        [HttpGet("enums/statuses")]
        public IActionResult GetInstallmentStatuses()
        {
            var statuses = Enum.GetValues(typeof(InstallmentStatus))
                .Cast<InstallmentStatus>()
                .Select(s => new { Value = (int)s, Name = s.ToString() });
            return Ok(statuses);
        }
    }
}
