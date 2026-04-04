using System;

namespace Accounting.Domain.Entities
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SellRate { get; set; }
        public decimal? OfficialRate { get; set; }
        public string Source { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Currency Currency { get; set; } = null!;
    }
}
