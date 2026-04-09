using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using Accounting.Application.Features.Partners.Commands.CreatePartner;
using Accounting.Application.Features.Partners.Commands.UpdatePartner;
using Accounting.Application.Features.Partners.Queries.GetPartnerById;
using Accounting.Application.Features.Partners.Queries.GetPartnersList;
using SharedKernel.Core.Constants.Permissions;

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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetPartnersListQuery();
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
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
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
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

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpPut("{id:int}")]
        [HasPermission(AccountingPermissions.PartnersCreate)] // Same as Create based on specific restrictions requested
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePartnerCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest("ID in route must match ID in body.");
            }

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
