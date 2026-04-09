using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using Accounting.Application.Posting.Commands.PostTransaction;
using SharedKernel.Core.Constants.Permissions;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/posting")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]

    public class PostingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.PostingExecute)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostTransaction([FromBody] PostTransactionCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(new { success = true, message = result.Message, data = result.Data });
        }
    }
}
