using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Features.ReportFeatures.RunReportWithAiPrompet
{
    public class RunReportWithAiPromptRequest :IRequest<RunReportWithAiPromptResponse>
    {
        public int ReportId { get; set; }
        public string Prompt { get; set; } = string.Empty;
    }
}
