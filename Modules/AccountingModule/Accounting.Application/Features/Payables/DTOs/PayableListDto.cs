using Accounting.Domain.Enums;

namespace Accounting.Application.Features.Payables.DTOs
{
    public class PayableListDto
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public DateTime DueDate { get; set; }
        public PayableStatus Status { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
