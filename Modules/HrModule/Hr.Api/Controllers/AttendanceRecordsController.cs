using Hr.Application.Features.AttendanceRecordFeatures.CreateAttendanceRecord;
using Hr.Application.Features.AttendanceRecordFeatures.DeleteAttendanceRecord;
using Hr.Application.Features.AttendanceRecordFeatures.GetAllAttendanceRecords;
using Hr.Application.Features.AttendanceRecordFeatures.GetAttendanceRecordById;
using Hr.Application.Features.AttendanceRecordFeatures.GetAttendanceRecordsPaged;
using Hr.Application.Features.AttendanceRecordFeatures.UpdateAttendanceRecord;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class AttendanceRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendanceRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAttendanceRecordRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllAttendanceRecordsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetAttendanceRecordsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAttendanceRecordByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.AttendanceRecord == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendanceRecordRequest request)
        {
            if (id != request.RecordId)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteAttendanceRecordRequest { RecordId = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
