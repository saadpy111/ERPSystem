namespace Report.Application.DTOs
{
    public class ReportDto
    {
        public int ReportId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Query { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public ReportDataSourceDto? ReportDataSource { get; set; }
        public List<ReportFieldDto> Fields { get; set; } = new List<ReportFieldDto>();
        public List<ReportParameterDto> Parameters { get; set; } = new List<ReportParameterDto>();
        public List<ReportFilterDto> Filters { get; set; } = new List<ReportFilterDto>();
        public List<ReportGroupDto> Groups { get; set; } = new List<ReportGroupDto>();
        public List<ReportSortingDto> Sortings { get; set; } = new List<ReportSortingDto>();
    }
}