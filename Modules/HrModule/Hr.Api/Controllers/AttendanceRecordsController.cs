using Hr.Application.Features.AttendanceRecordFeatures.CreateAttendanceRecord;
using Hr.Application.Features.AttendanceRecordFeatures.DeleteAttendanceRecord;
using Hr.Application.Features.AttendanceRecordFeatures.GetAllAttendanceRecords;
using Hr.Application.Features.AttendanceRecordFeatures.GetAttendanceRecordById;
using Hr.Application.Features.AttendanceRecordFeatures.GetAttendanceRecordsPaged;
using Hr.Application.DTOs;
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
            request.RecordId = id;

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

        [HttpGet("AttendanceRecordMetadata")]
        public IActionResult GetMetadata()
        {
            var metadata = new EntityMetadataDto
            {
                EntityName = "AttendanceRecord",
                OrderableFields = new List<OrderableFieldDto>
                {
                    new OrderableFieldDto { Key = "Date", Label = "Date" },
                    new OrderableFieldDto { Key = "CheckInTime", Label = "Check In Time" },
                    new OrderableFieldDto { Key = "CheckOutTime", Label = "Check Out Time" },
                    new OrderableFieldDto { Key = "DelayMinutes", Label = "Delay Minutes" }
                },
                FilterableFields = new List<FilterableFieldDto>
                {
                    new FilterableFieldDto { Key = "searchTerm", Type = "string" },
                    new FilterableFieldDto { Key = "employeeId", Type = "number" },
                    new FilterableFieldDto { Key = "startDate", Type = "date" },
                    new FilterableFieldDto { Key = "endDate", Type = "date" }
                },
                Pagination = new PaginationMetadataDto
                {
                    DefaultPageSize = 10,
                    MaxPageSize = 100
                }
            };

            return Ok(metadata);
        }
    }
}
