namespace Report.Application.DTOs
{
    public class ReportFilterDto
    {
        public int ReportFilterId { get; set; }
        public int ReportId { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string ParameterName { get; set; } = string.Empty;
    }
}