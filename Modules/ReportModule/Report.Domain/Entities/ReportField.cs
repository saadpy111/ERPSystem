using Report.Domain.Enums;

namespace Report.Domain.Entities
{
    public class ReportField : BaseEntity
    {
        public int ReportFieldId { get; set; }

        public int ReportId { get; set; }
        public Report Report { get; set; } = default!;

        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; }

        public string Expression { get; set; } = string.Empty;

        public FieldType Type { get; set; }
        public int? Width { get; set; }
        public bool IsVisible { get; set; } = true;
    }
}