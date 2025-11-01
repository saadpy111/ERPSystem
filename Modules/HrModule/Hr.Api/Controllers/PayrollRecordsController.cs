using Hr.Application.Features.PayrollRecordFeatures.CreatePayrollRecord;
using Hr.Application.Features.PayrollRecordFeatures.DeletePayrollRecord;
using Hr.Application.Features.PayrollRecordFeatures.GetAllPayrollRecords;
using Hr.Application.Features.PayrollRecordFeatures.GetPayrollRecordById;
using Hr.Application.Features.PayrollRecordFeatures.GetPayrollRecordsPaged;
using Hr.Application.Features.PayrollRecordFeatures.UpdatePayrollRecord;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class PayrollRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayrollRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePayrollRecordRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllPayrollRecordsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetPayrollRecordsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetPayrollRecordByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.PayrollRecord == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePayrollRecordRequest request)
        {
            if (id != request.PayrollId)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeletePayrollRecordRequest { PayrollId = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Enum Endpoints

        [HttpGet("enums/statuses")]
        public IActionResult GetPayrollStatuses()
        {
            var statuses = Enum.GetValues(typeof(PayrollStatus))
                .Cast<PayrollStatus>()
                .Select(s => new { Value = (int)s, Name = s.ToString() });
            return Ok(statuses);
        }
    }
}
