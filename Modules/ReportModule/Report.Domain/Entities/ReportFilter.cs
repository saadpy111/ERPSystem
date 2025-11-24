using Report.Domain.Enums;

namespace Report.Domain.Entities
{
    public class ReportFilter : BaseEntity
    {
        public int ReportFilterId { get; set; }

        public int ReportId { get; set; }
        public Report Report { get; set; } = default!;

        public string FieldName { get; set; } = string.Empty;

        public FilterOperator Operator { get; set; }

        // Parameter input name (runtime value comes from user)
        public string ParameterName { get; set; } = string.Empty;
    }
}
