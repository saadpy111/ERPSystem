using System;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public int DecimalPlaces { get; set; }
        public bool IsBaseCurrency { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<CurrencyRate> CurrencyRates { get; set; } = new List<CurrencyRate>();
    }
}
