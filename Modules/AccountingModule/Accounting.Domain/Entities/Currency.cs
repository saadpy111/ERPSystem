using Accounting.Domain.Common;
using System.Collections.Generic;

namespace Accounting.Domain.Entities
{
    public class Currency : BaseEntity
    {
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public int DecimalPlaces { get; set; }
        public bool IsBaseCurrency { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<CurrencyRate> CurrencyRates { get; set; } = new List<CurrencyRate>();
    }
}
