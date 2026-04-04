using Accounting.Domain.Enums;

namespace Accounting.Domain.Entities
{
    public class AccountingMapping
    {
        public int Id { get; set; }
        public SourceType SourceType { get; set; }
        public string MappingKey { get; set; } = null!;
        public int AccountId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
