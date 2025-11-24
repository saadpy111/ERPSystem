using System.Collections.Generic;

namespace Report.Domain.Entities
{
    public class Report : BaseEntity
    {
        public int ReportId { get; set; }

        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }

        public string Query { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<ReportField> Fields { get; set; } = new List<ReportField>();
        public ICollection<ReportParameter> Parameters { get; set; } = new List<ReportParameter>();
        public ICollection<ReportFilter> Filters { get; set; } = new List<ReportFilter>();
        public ICollection<ReportGroup> Groups { get; set; } = new List<ReportGroup>();
        public ICollection<ReportSorting> Sortings { get; set; } = new List<ReportSorting>();
    }
}