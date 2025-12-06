namespace Report.Application.DTOs
{
    public class ReportFieldDto
    {
        public int ReportFieldId { get; set; }
        public int ReportId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string Expression { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int? Width { get; set; }
        public bool IsVisible { get; set; }
    }
}