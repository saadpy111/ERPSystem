using Accounting.Domain.Common;
using Accounting.Domain.Enums;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class Partner : BaseEntity
    {
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public PartnerType Type { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
        public string? ContactPerson { get; set; }
        public string? PaymentTerms { get; set; }
        public decimal? CreditLimit { get; set; }
        public int DefaultCurrencyId { get; set; }
        public int? DefaultTaxId { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public string? PostalCode { get; set; }
        public bool IsActive { get; set; }
        public virtual Currency DefaultCurrency { get; set; } = null!;
        public virtual Tax? DefaultTax { get; set; }
        public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
        public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
    }
}
