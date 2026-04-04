using Accounting.Application.Features.JournalEntries.DTOs;
using MediatR;

namespace Accounting.Application.Features.JournalEntries.Queries.GetJournalEntryById
{
    public class GetJournalEntryByIdQuery : IRequest<JournalEntryResponseDto>
    {
        public int Id { get; set; }
    }
}
