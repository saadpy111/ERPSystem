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
using SharedKernel.Core.Constants.Permissions;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/accounting/journal-entries")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "Accounting")]

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
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [HasPermission(AccountingPermissions.JournalEntriesView)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetJournalEntryByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        [HasPermission(AccountingPermissions.JournalEntriesCreate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateJournalEntryCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("post")]
        [HasPermission(AccountingPermissions.JournalEntriesPost)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostEntry([FromBody] PostJournalEntryCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

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

            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }

    public sealed class ReverseJournalEntryRequest
    {
        public string Reason { get; set; } = null!;
    }
}
