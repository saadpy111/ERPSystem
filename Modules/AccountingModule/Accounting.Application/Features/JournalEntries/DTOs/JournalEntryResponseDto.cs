using Accounting.Domain.Enums;
using System.Collections.Generic;

namespace Accounting.Application.Features.JournalEntries.DTOs
{
    public class JournalEntryResponseDto
    {
        public int Id { get; set; }
        public JournalStatus Status { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public string? Description { get; set; }
        public string? Reference { get; set; }
        public List<JournalEntryLineResponseDto> Lines { get; set; } = new List<JournalEntryLineResponseDto>();
    }

    public class JournalEntryLineResponseDto
    {
        public int AccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string? Description { get; set; }
    }
}
