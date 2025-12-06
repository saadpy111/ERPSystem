namespace Report.Application.DTOs
{
    public class ReportParameterDto
    {
        public int ReportParameterId { get; set; }
        public int ReportId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string DataType { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public string? DefaultValue { get; set; }
    }
}