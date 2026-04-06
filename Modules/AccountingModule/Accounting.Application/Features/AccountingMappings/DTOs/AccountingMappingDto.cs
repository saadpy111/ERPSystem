using Accounting.Domain.Enums;

namespace Accounting.Application.Features.AccountingMappings.DTOs
{
    public class AccountingMappingDto
    {
        public int Id { get; set; }
        public SourceType SourceType { get; set; }
        public string MappingKey { get; set; } = null!;
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}
