using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Features.ReportFeatures.RunReportWithAiPrompet
{
    public class RunReportWithAiPromptResponse
    {
        public bool Success { get; set; } = true;
        public  string Message  { get; set; }
        public object Data { get; set; } = default!;
    }
}
