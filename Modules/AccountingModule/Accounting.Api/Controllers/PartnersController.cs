using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using Accounting.Application.Features.Partners.Commands.CreatePartner;
using Accounting.Application.Features.Partners.Commands.UpdatePartner;
using Accounting.Application.Features.Partners.Commands.DeletePartner;
using Accounting.Application.Features.Partners.Commands.TogglePartnerStatus;
using Accounting.Application.Features.Partners.Queries.GetPartnerById;
using Accounting.Application.Features.Partners.Queries.GetPartnersList;
using SharedKernel.Core.Constants.Permissions;
using Accounting.Domain.Enums;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/partners")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class PartnersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PartnersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [HasPermission(AccountingPermissions.PartnersView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] PartnerType? type, CancellationToken cancellationToken)
        {
            var query = new GetPartnersListQuery { Type = type };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, data = result.Data });
        }

        [HttpGet("{id:int}")]
        [HasPermission(AccountingPermissions.PartnersView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetPartnerByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success)
            {
                return NotFound(new { result.Message });
            }

            return Ok(new { success = true, data = result.Data });
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.PartnersCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePartnerCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, new { success = true, message = result.Message, data = result.Data });
        }

        [HttpPut("{id:int}")]
        [HasPermission(AccountingPermissions.PartnersEdit)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePartnerCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in route must match ID in body.");
            }

            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AccountingPermissions.PartnersDelete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var command = new DeletePartnerCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }

        [HttpPatch("{id:int}/toggle-status")]
        [HasPermission(AccountingPermissions.PartnersEdit)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
        {
            var command = new TogglePartnerStatusCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message });
        }
    }
}
