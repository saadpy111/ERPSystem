using Hr.Domain.Enums;

namespace Hr.Application.DTOs
{
    public class SalaryStructureComponentForUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public PayrollComponentType Type { get; set; }
        public decimal? FixedAmount { get; set; }
        public decimal? Percentage { get; set; }
    }
}