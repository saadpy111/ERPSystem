namespace Report.Application.DTOs
{
    public class ReportDataSourceDto
    {
        public int ReportDataSourceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string? SqlTemplate { get; set; }
    }
}