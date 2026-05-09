using Accounting.Application.Common.Models;
using MediatR;
using System;

namespace Accounting.Application.Features.JournalEntries.Commands.ReverseJournalEntry
{
    public class ReverseJournalEntryCommand : IRequest<Result<int>>
    {
        public int JournalEntryId { get; set; }
        public DateTime ReversalDate { get; set; } = DateTime.UtcNow;
        public string? Reason { get; set; }

        /// <summary>
        /// The actor performing the reversal — stored as ReversedBy on the reversal JournalEntry.
        /// The original journal is never modified.
        /// </summary>
        public string? ReversedBy { get; set; }
    }
}
