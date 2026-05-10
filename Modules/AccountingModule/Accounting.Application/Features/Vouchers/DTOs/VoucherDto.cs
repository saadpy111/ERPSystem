using Accounting.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Accounting.Application.Features.Vouchers.DTOs
{
    public class VoucherDto
    {
        public int Id { get; set; }
        public string VoucherNumber { get; set; } = null!;
        public VoucherType VoucherType { get; set; }
        public DateTime Date { get; set; }
        public VoucherStatus Status { get; set; }
        public int? PartnerId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public int CurrencyId { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
        public int? JournalEntryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public List<VoucherLineDto> Lines { get; set; } = new();
    }

    public class VoucherLineDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int CurrencyId { get; set; }
    }
}
