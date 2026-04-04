using Accounting.Application.Features.JournalEntries.Commands.ReverseJournalEntry;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/journal-entries")]
    [Produces("application/json")]
    public class JournalEntriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JournalEntriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Reverses a Posted journal entry.
        /// </summary>
        /// <remarks>
        /// Creates a new mirror journal entry with every debit/credit swapped and marks
        /// the original as <c>Reversed</c>. The original entry is never deleted.
        ///
        /// Business rules enforced:
        /// - Entry must exist and be in <c>Posted</c> status.
        /// - Entry must not have been reversed before.
        /// - The entry's fiscal period must be open.
        /// - Reason must be provided (audit trail).
        /// </remarks>
        /// <param name="id">ID of the Posted journal entry to reverse.</param>
        /// <param name="request">Reversal payload (only <c>Reason</c> is required).</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The ID of the newly created reversal journal entry.</returns>
        [HttpPost("{id:int}/reverse")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Reverse(
            [FromRoute] int id,
            [FromBody] ReverseJournalEntryRequest request,
            CancellationToken cancellationToken)
        {
            var command = new ReverseJournalEntryCommand
            {
                JournalEntryId = id,
                Reason         = request.Reason,
            };

            var newReversalId = await _mediator.Send(command, cancellationToken);
            return Ok(newReversalId);
        }
    }

    /// <summary>Request body for the reversal endpoint.</summary>
    public sealed class ReverseJournalEntryRequest
    {
        /// <summary>Mandatory business reason for the reversal (stored as an audit trail).</summary>
        public string Reason { get; set; } = null!;
    }
}
