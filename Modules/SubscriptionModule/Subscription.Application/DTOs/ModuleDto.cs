namespace Subscription.Application.DTOs
{
    public class ModuleDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ModulePriceDto> Prices { get; set; } = new();
    }

    public class ModulePriceDto
    {
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string Interval { get; set; } = string.Empty;
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    public class PurchasedModuleDto
    {
        public string Id { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string Interval { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool AutoRenew { get; set; }
    }
}
