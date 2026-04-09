using Accounting.Domain.Enums;
using MediatR;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.JournalEntries.Commands.PostJournalEntry
{
    public class PostJournalEntryCommand : IRequest<Result<int>>
    {
        public int JournalEntryId { get; set; }
    }
}
