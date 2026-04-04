using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Posting.Commands.PostTransaction;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/posting")]
    [Produces("application/json")]
    public class PostingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostTransaction([FromBody] PostTransactionCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
