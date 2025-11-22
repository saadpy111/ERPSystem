using Hr.Domain.Enums;

namespace Hr.Application.DTOs
{
    public class SalaryStructureComponentForCreationDto
    {
        public string Name { get; set; } = string.Empty;
        public PayrollComponentType Type { get; set; } 
        public decimal? FixedAmount { get; set; }
        public decimal? Percentage { get; set; }
    }
}