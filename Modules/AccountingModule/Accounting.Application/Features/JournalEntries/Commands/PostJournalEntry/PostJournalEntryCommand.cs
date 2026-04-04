using Accounting.Domain.Enums;
using MediatR;

namespace Accounting.Application.Features.JournalEntries.Commands.PostJournalEntry
{
    public class PostJournalEntryCommand : IRequest<int>
    {
        public int JournalEntryId { get; set; }
    }
}
