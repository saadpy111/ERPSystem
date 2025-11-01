using MediatR;

namespace Hr.Application.Features.PayrollComponentFeatures.GetPayrollComponentsPaged
{
    public class GetPayrollComponentsPagedRequest : IRequest<GetPayrollComponentsPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "Name";
        public bool IsDescending { get; set; } = false;
        public int? PayrollRecordId { get; set; }
        public string? ComponentType { get; set; }
    }
}
