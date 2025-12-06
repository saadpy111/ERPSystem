using Report.Domain.Enums;

namespace Report.Domain.Entities
{
    public class ReportDataSource : BaseEntity
    {
        public int ReportDataSourceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DataSourceType Type { get; set; }
        public string? SqlTemplate { get; set; }
    }

}
