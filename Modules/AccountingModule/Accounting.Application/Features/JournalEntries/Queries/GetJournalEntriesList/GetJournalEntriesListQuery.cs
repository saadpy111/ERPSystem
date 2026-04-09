using Accounting.Application.Common.Models;
using Accounting.Application.Features.JournalEntries.DTOs;
using MediatR;
using System;

namespace Accounting.Application.Features.JournalEntries.Queries.GetJournalEntriesList
{
    public class GetJournalEntriesListQuery : IRequest<Result<PagedResult<JournalEntryResponseDto>>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
