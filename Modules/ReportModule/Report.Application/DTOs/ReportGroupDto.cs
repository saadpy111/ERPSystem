namespace Report.Application.DTOs
{
    public class ReportGroupDto
    {
        public int ReportGroupId { get; set; }
        public int ReportId { get; set; }
        public string Expression { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}