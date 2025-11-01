using Hr.Application.Features.LeaveRequestFeatures.CreateLeaveRequest;
using Hr.Application.Features.LeaveRequestFeatures.DeleteLeaveRequest;
using Hr.Application.Features.LeaveRequestFeatures.GetAllLeaveRequests;
using Hr.Application.Features.LeaveRequestFeatures.GetLeaveRequestById;
using Hr.Application.Features.LeaveRequestFeatures.GetLeaveRequestsPaged;
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

        [HttpGet("enums/leave-types")]
        public IActionResult GetLeaveTypes()
        {
            var leaveTypes = Enum.GetValues(typeof(LeaveType))
                .Cast<LeaveType>()
                .Select(l => new { Value = (int)l, Name = l.ToString() });
            return Ok(leaveTypes);
        }
    }
}
