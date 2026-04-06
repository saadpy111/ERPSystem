using Accounting.Domain.Common;
using System;

namespace Accounting.Domain.Entities
{
    public class CurrencyRate : BaseEntity
    {
        public int CurrencyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SellRate { get; set; }
        public decimal? OfficialRate { get; set; }
        public string Source { get; set; } = null!;
        public virtual Currency Currency { get; set; } = null!;
    }
}
