using Identity.Application.Features.RoleFeature.Commands.CreateRole;
using Identity.Application.Features.RoleFeature.Commands.DeleteRole;
using Identity.Application.Features.RoleFeature.Commands.UpdateRole;
using Identity.Application.Features.RoleFeature.Queries.GetAllRoles;
using Identity.Application.Features.RoleFeature.Queries.GetRoleById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Identity")]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/Role
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandRequest request)
        {
            var res = await _mediator.Send(request);
            if (!res.Success) return BadRequest(res.Errors);
            return CreatedAtAction(nameof(GetRoleById), new { id = res.Role!.Id }, res.Role);
        }

        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> GetAllRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50, [FromQuery] string? search = null)
        {
            var query = new GetAllRolesQueryRequest { PageNumber = pageNumber, PageSize = pageSize, Search = search };
            var res = await _mediator.Send(query);
            return Ok(res.PagedResult);
        }

        // GET: api/Role/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var query = new GetRoleByIdQueryRequest { Id = id };
            var res = await _mediator.Send(query);
            if (res.Role == null) return NotFound();
            return Ok(res.Role);
        }

        // PUT: api/Role/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleCommandRequest request)
        {
            request.Id = id;
            var res = await _mediator.Send(request);
            if (!res.Success) return BadRequest(res.Errors);
            return Ok(res.Role);
        }

        // DELETE: api/Role/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var command = new DeleteRoleCommandRequest { Id = id };
            var res = await _mediator.Send(command);
            if (!res.Success) return BadRequest(res.Errors);
            return NoContent();
        }
    }
}