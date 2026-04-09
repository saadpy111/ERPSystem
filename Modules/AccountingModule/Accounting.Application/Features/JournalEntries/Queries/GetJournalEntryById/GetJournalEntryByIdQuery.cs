using Accounting.Application.Features.JournalEntries.DTOs;
using MediatR;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.JournalEntries.Queries.GetJournalEntryById
{
    public class GetJournalEntryByIdQuery : IRequest<Result<JournalEntryResponseDto>>
    {
        public int Id { get; set; }
    }
}
