using Accounting.Application.Common.Models;
using MediatR;
using System;

namespace Accounting.Application.Features.JournalEntries.Commands.ReverseJournalEntry
{
    public class ReverseJournalEntryCommand : IRequest<Result<int>>
    {
        public int JournalEntryId { get; set; }
        public DateTime ReversalDate { get; set; }
        public string? Reason { get; set; }
    }
}
