using MediatR;

namespace Hr.Application.Features.LoanFeatures.GetLoansPaged
{
    public class GetLoansPagedRequest : IRequest<GetLoansPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "StartDate";
        public bool IsDescending { get; set; } = false;
        public int? EmployeeId { get; set; }
        public string? Status { get; set; }
    }
}
