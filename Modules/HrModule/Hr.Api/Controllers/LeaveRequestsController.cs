using Hr.Application.Features.LeaveRequestFeatures.CreateLeaveRequest;
using Hr.Application.Features.LeaveRequestFeatures.DeleteLeaveRequest;
using Hr.Application.Features.LeaveRequestFeatures.GetAllLeaveRequests;
using Hr.Application.Features.LeaveRequestFeatures.GetLeaveRequestById;
using Hr.Application.Features.LeaveRequestFeatures.GetLeaveRequestsPaged;
using Hr.Application.DTOs;
using Hr.Application.Features.LeaveRequestFeatures.UpdateLeaveRequest;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLeaveRequestRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllLeaveRequestsRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetLeaveRequestsPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetLeaveRequestByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.LeaveRequest == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLeaveRequestRequest request)
        {
            request.Id = id;

            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteLeaveRequestRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Enum Endpoints

        [HttpGet("enums/statuses")]
        public IActionResult GetLeaveRequestStatuses()
        {
            var statuses = Enum.GetValues(typeof(LeaveRequestStatus))
                .Cast<LeaveRequestStatus>()
                .Select(s => new { Value = (int)s, Name = s.ToString() });
            return Ok(statuses);
        }

        [HttpGet("LeaveRequestMetadata")]
        public IActionResult GetMetadata()
        {
            var metadata = new EntityMetadataDto
            {
                EntityName = "LeaveRequest",
                OrderableFields = new List<OrderableFieldDto>
                {
                    new OrderableFieldDto { Key = "StartDate", Label = "Start Date" },
                    new OrderableFieldDto { Key = "EndDate", Label = "End Date" },
                    new OrderableFieldDto { Key = "DurationDays", Label = "Duration Days" },
                    new OrderableFieldDto { Key = "Status", Label = "Status" },
                    new OrderableFieldDto { Key = "LeaveTypeName", Label = "Leave Type Name" }
                },
                FilterableFields = new List<FilterableFieldDto>
                {
                    new FilterableFieldDto { Key = "searchTerm", Type = "string" },
                    new FilterableFieldDto { Key = "employeeId", Type = "number" },
                    new FilterableFieldDto { Key = "leaveType", Type = "string" },
                    new FilterableFieldDto { Key = "status", Type = "enum", Values = Enum.GetNames(typeof(LeaveRequestStatus)).ToList() }
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
