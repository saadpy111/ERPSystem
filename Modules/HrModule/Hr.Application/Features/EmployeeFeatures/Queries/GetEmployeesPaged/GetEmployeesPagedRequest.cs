using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeesPaged
{
    public class GetEmployeesPagedRequest : IRequest<GetEmployeesPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "FullName";
        public bool IsDescending { get; set; } = false;
        public string? Status { get; set; }
    }
}
