using Accounting.Domain.Common;
using Accounting.Domain.Enums;

namespace Accounting.Domain.Entities
{
    public class AccountingMapping : BaseEntity
    {
        public SourceType SourceType { get; set; }
        public string MappingKey { get; set; } = null!;
        public int AccountId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
