using Accounting.Domain.Enums;
using System;

namespace Accounting.Application.Posting.Interfaces
{
    public interface IPostingRequest
    {
        SourceType SourceType { get; set; }
        int SourceId { get; set; }
        int CurrencyId { get; set; }
        DateTime Date { get; set; }
        string? Description { get; set; }
        string? Reference { get; set; }

        /// <summary>
        /// The identity (username / user-id) of the actor triggering this posting.
        /// Stored as PostedBy on the JournalEntry for immutable audit trail.
        /// </summary>
        string? PostedBy { get; set; }
    }
}
