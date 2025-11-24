using Report.Domain.Enums;

namespace Report.Domain.Entities
{
    public class ReportSorting : BaseEntity
    {
        public int ReportSortingId { get; set; }

        public int ReportId { get; set; }
        public Report Report { get; set; } = default!;

        public string Expression { get; set; } = string.Empty;

        public SortDirection Direction { get; set; } = SortDirection.Ascending;

        public int SortOrder { get; set; } = 0;
    }
}