using MediatR;

namespace Hr.Application.Features.PayrollRecordFeatures.GetPayrollRecordsPaged
{
    public class GetPayrollRecordsPagedRequest : IRequest<GetPayrollRecordsPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "PeriodYear";
        public bool IsDescending { get; set; } = false;
        public int? EmployeeId { get; set; }
        public int? PeriodYear { get; set; }
        public int? PeriodMonth { get; set; }
        public string? Status { get; set; }
    }
}
