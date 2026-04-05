using Accounting.Application.Features.JournalEntries.Commands.CreateJournalEntry;
using Accounting.Application.Features.JournalEntries.Commands.PostJournalEntry;
using Accounting.Application.Features.JournalEntries.Commands.ReverseJournalEntry;
using Accounting.Application.Features.JournalEntries.Queries.GetJournalEntriesList;
using Accounting.Application.Features.JournalEntries.Queries.GetJournalEntryById;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Authorization;
using SharedKernel.Constants.Permissions;

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

        [HttpGet]
        [HasPermission(AccountingPermissions.JournalEntriesView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var query = new GetJournalEntriesListQuery { StartDate = startDate, EndDate = endDate, PageNumber = pageNumber, PageSize = pageSize };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [HasPermission(AccountingPermissions.JournalEntriesView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetJournalEntryByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.JournalEntriesCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateJournalEntryCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result }, result);
        }

        [HttpPost("post")]
        [HasPermission(AccountingPermissions.JournalEntriesPost)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostEntry([FromBody] PostJournalEntryCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
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
        [HasPermission(AccountingPermissions.JournalEntriesReverse)]
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
