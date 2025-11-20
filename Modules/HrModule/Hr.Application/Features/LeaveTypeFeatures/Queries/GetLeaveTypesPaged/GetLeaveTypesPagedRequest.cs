using MediatR;

namespace Hr.Application.Features.LeaveTypeFeatures.Queries.GetLeaveTypesPaged
{
    public class GetLeaveTypesPagedRequest : IRequest<GetLeaveTypesPagedResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; } = "LeaveTypeName";
        public bool IsDescending { get; set; } = false;
        public string? Status { get; set; }
    }
}