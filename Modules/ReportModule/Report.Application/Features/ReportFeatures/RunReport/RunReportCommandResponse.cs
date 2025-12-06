using Report.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Features.ReportFeatures.RunReport
{
    public class RunReportCommandResponse 
    {
        public IEnumerable<IDictionary<string, object?>> Data { get; set; } = new List<IDictionary<string, object?>>();
        public IReadOnlyList<FieldMeta> Fields { get; set; } = new List<FieldMeta>();
        public long? TotalCount { get; set; }
    }
}
