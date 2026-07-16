using MediatR;

namespace Website.Application.Features.WalletFeatures.Queries.GetMyWithdrawalRequests
{
    public class GetMyWithdrawalRequestsQuery : IRequest<GetMyWithdrawalRequestsResponse>
    {
        public string? StatusFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class MyWithdrawalRequestDto
    {
        public string Id { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewedBy { get; set; }
        public string? Notes { get; set; }
    }

    public class GetMyWithdrawalRequestsResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public List<MyWithdrawalRequestDto> Requests { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
