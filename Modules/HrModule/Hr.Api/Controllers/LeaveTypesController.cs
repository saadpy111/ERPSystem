using Hr.Application.Features.LeaveTypeFeatures.Commands.CreateLeaveType;
using Hr.Application.Features.LeaveTypeFeatures.Commands.DeleteLeaveType;
using Hr.Application.Features.LeaveTypeFeatures.Commands.UpdateLeaveType;
using Hr.Application.Features.LeaveTypeFeatures.Queries.GetAllLeaveTypes;
using Hr.Application.Features.LeaveTypeFeatures.Queries.GetLeaveTypeById;
using Hr.Application.Features.LeaveTypeFeatures.Queries.GetLeaveTypesPaged;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Hr")]
    public class LeaveTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLeaveTypeRequest request)
        {
            var result = await _mediator.Send(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllLeaveTypesRequest();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] GetLeaveTypesPagedRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetLeaveTypeByIdRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.LeaveType == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLeaveTypeRequest request)
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
            var request = new DeleteLeaveTypeRequest { Id = id };
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Enum Endpoints

        [HttpGet("enums/statuses")]
        public IActionResult GetLeaveTypeStatuses()
        {
            var statuses = Enum.GetValues(typeof(LeaveTypeStatus))
                .Cast<LeaveTypeStatus>()
                .Select(s => new { Value = (int)s, Name = s.ToString() });
            return Ok(statuses);
        }
    }
}