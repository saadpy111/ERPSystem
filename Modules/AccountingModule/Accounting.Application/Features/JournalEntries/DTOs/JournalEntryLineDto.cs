namespace Accounting.Application.Features.JournalEntries.DTOs
{
    public class JournalEntryLineDto
    {
        public int AccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string? Description { get; set; }
        public int? PartnerId { get; set; }
    }
}
