namespace Report.Domain.Entities
{
    public class ReportGroup : BaseEntity
    {
        public int ReportGroupId { get; set; }

        public int ReportId { get; set; }
        public Report Report { get; set; } = default!;

        public string Expression { get; set; } = string.Empty;

        public int SortOrder { get; set; } = 0;
    }
}
