using MediatR;

namespace Hr.Application.Features.LoanInstallmentFeatures.GetLoanInstallmentsPaged
{
    public class GetLoanInstallmentsPagedRequest : IRequest<GetLoanInstallmentsPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "DueDate";
        public bool IsDescending { get; set; } = false;
        public int? LoanId { get; set; }
        public string? Status { get; set; }
    }
}
