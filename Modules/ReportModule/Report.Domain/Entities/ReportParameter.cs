using Report.Domain.Enums;

namespace Report.Domain.Entities
{
    public class ReportParameter : BaseEntity
    {
        public int ReportParameterId { get; set; }

        public int ReportId { get; set; }
        public Report Report { get; set; } = default!;

        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; }

        public ParameterDataType DataType { get; set; }
        public bool IsRequired { get; set; }
        public string? DefaultValue { get; set; }
    }
}