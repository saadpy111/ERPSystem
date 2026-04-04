using MediatR;

namespace Accounting.Application.Features.JournalEntries.Commands.ReverseJournalEntry
{
    public class ReverseJournalEntryCommand : IRequest<int>
    {
        /// <summary>The ID of the Posted journal entry to be reversed.</summary>
        public int JournalEntryId { get; set; }

        /// <summary>Mandatory business reason for the reversal (audit trail).</summary>
        public string Reason { get; set; } = null!;
    }
}
