using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using Accounting.Application.Features.CostCenters.Commands.CreateCostCenter;
using Accounting.Application.Features.CostCenters.Queries.GetCostCentersList;
using SharedKernel.Core.Constants.Permissions;

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

        [HttpGet]
        [HasPermission(AccountingPermissions.CostCentersView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetCostCentersListQuery();
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.CostCentersCreate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCostCenterCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }
    }
}
