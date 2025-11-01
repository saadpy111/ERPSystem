using Hr.Domain.Enums;

namespace Hr.Application.DTOs
{
    public class PayrollComponentDto
    {
        public int ComponentId { get; set; }
        public int PayrollRecordId { get; set; }
        public string Name { get; set; } = string.Empty;
        public PayrollComponentType ComponentType { get; set; }
        public decimal Amount { get; set; }
    }
}
