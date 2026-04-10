using Accounting.Application.Features.CostCenters.Commands.CreateCostCenter;
using Accounting.Application.Features.CostCenters.Commands.DeleteCostCenter;
using Accounting.Application.Features.CostCenters.Commands.UpdateCostCenter;
using Accounting.Application.Features.CostCenters.Queries.GetCostCenterById;
using Accounting.Application.Features.CostCenters.Queries.GetCostCentersTree;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Authorization;
using SharedKernel.Core.Constants.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/cost-centers")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]
    public class CostCentersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CostCentersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("tree")]
        [HasPermission(AccountingPermissions.CostCentersView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTree(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCostCentersTreeQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [HasPermission(AccountingPermissions.CostCentersView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCostCenterByIdQuery { Id = id }, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.CostCentersCreate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateCostCenterCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [HasPermission(AccountingPermissions.CostCentersEdit)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCostCenterCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id) return BadRequest("ID mismatch");
            var result = await _mediator.Send(command, cancellationToken);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AccountingPermissions.CostCentersDelete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteCostCenterCommand { Id = id }, cancellationToken);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
