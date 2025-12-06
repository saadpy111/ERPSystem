namespace Report.Application.DTOs
{
    public class ReportSortingDto
    {
        public int ReportSortingId { get; set; }
        public int ReportId { get; set; }
        public string Expression { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}