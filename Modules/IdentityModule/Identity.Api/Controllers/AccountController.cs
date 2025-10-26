using Identity.Application.Features.AccountFeature.Commands.AssignRoles;
using Identity.Application.Features.AccountFeature.Commands.DeleteAccount;
using Identity.Application.Features.AccountFeature.Commands.RemoveRole;
using Identity.Application.Features.AccountFeature.Commands.UpdateAccount;
using Identity.Application.Features.AccountFeature.Queries.GetAccountById;
using Identity.Application.Features.AccountFeature.Queries.GetAllAcounts;
using Identity.Application.Features.AccountFeature.Queries.GetUserRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [ApiExplorerSettings(GroupName = "Identity")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all users with their roles (with optional search and pagination)
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAccounts(
            [FromQuery] string? searchText = "",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllAccountsQueryRequest
            {
                Search = searchText,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            return Ok(result.PagedResult);
        }

        /// <summary>
        /// Get single user by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            var query = new GetAccountByIdQueryRequest { Id = id };
            var result = await _mediator.Send(query);

            if (result.Account == null)
                return NotFound();

            return Ok(result.Account);
        }

        /// <summary>
        /// Update user info (Email, FullName)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] UpdateAccountCommandRequest request)
        {
            if (request == null) return BadRequest("Request body is required.");

            request.Id = id;
            var response = await _mediator.Send(request);

            if (!response.Success)
                return BadRequest(response.Errors);

            return Ok(response.Account);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var command = new DeleteAccountCommandRequest { Id = id };
            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response.Errors);

            return NoContent();
        }

        /// <summary>
        /// Assign roles to a user
        /// POST body: ["Role1","Role2"]
        /// </summary>
        [HttpPost("{id}/roles")]
        public async Task<IActionResult> AssignRoles(string id, [FromBody] List<string> roles)
        {
            if (roles == null || roles.Count == 0)
                return BadRequest("At least one role must be provided.");

            var command = new AssignRolesCommandRequest
            {
                UserId = id,
                Roles = roles
            };

            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response.Errors);

            return Ok(new { AssignedRoles = response.AssignedRoles });
        }

        /// <summary>
        /// Get roles assigned to a specific user
        /// </summary>
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var query = new GetUserRolesQueryRequest { UserId = id };
            var response = await _mediator.Send(query);

            if (response.Roles == null || response.Roles.Count == 0)
                return Ok(new List<string>()); // return empty list rather than NotFound to keep consistent list shape

            return Ok(response.Roles);
        }

        /// <summary>
        /// Remove a role from a user
        /// </summary>
        [HttpDelete("{id}/roles/{roleName}")]
        public async Task<IActionResult> RemoveRole(string id, string roleName)
        {
            var command = new RemoveRoleCommandRequest
            {
                UserId = id,
                Role = roleName
            };

            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response.Errors);

            return NoContent();
        }
    }
}