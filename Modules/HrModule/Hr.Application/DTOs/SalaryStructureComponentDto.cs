namespace Hr.Application.DTOs
{
    public class SalaryStructureComponentDto
    {
        public int Id { get; set; }
        public int SalaryStructureId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal? FixedAmount { get; set; }
        public decimal? Percentage { get; set; }
    }
}