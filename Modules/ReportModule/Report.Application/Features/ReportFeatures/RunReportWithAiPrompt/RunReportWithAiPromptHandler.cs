using MediatR;
using Report.Application.Contracts.Persistence.Repositories;
using Report.Application.Features.ReportFeatures.RunReportWithAiPrompet;
using Report.Application.Services;
using Report.Domain.Entities;
using System.Text;

public class RunReportWithAiPromptHandler
: IRequestHandler<RunReportWithAiPromptRequest, RunReportWithAiPromptResponse>
{
    private readonly IReportsRepository _reportsRepository;
    private readonly AIQueryBuilderService _ai;
    private readonly IReportQueryEngine _reportQueryEngine;

    public RunReportWithAiPromptHandler(
        IReportsRepository reportsRepository,
        AIQueryBuilderService ai,
        IReportQueryEngine reportQueryEngine)
    {
        _reportsRepository = reportsRepository;
        _ai = ai;
        _reportQueryEngine = reportQueryEngine;
    }

    public async Task<RunReportWithAiPromptResponse> Handle(
        RunReportWithAiPromptRequest request,
        CancellationToken cancellationToken)
    {
        try
        {

            var reportdetails = await _reportsRepository.GetFullReportAsync(request.ReportId);
            if (reportdetails == null)
                throw new Exception();
            var reportString = BuildAiSchemaFromReport(reportdetails);


            var sql = await _ai.GenerateSqlAsync(reportString, request.Prompt, cancellationToken);

            if (sql.Contains("delete", StringComparison.OrdinalIgnoreCase) ||
                sql.Contains("drop", StringComparison.OrdinalIgnoreCase) ||
                sql.Contains("update", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Unsafe SQL detected.");
            }
            var data = await _reportQueryEngine.ExecuteAsync(sql);

            return new RunReportWithAiPromptResponse
            {
                Data = data,
                Message = "تم عمل التقرير بنجاح"
            };
        }
        catch
        {
            return new RunReportWithAiPromptResponse()
            {
                Success = false,
                Message = "خطأ فالطلب"
            };
        }

    }
    

    private static string BuildAiSchemaFromReport(Report.Domain.Entities.Report report)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Table: {report.Query}");
        sb.AppendLine();
        sb.AppendLine("Columns:");

        foreach (var field in report.Fields)
        {
            sb.AppendLine($"{field.Name} ({field.Type})");
        }

        return sb.ToString();
    }

}
