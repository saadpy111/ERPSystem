using MediatR;

namespace Website.Application.Features.WalletFeatures.Queries.GetWithdrawalRequests
{
    public class GetWithdrawalRequestsQuery : IRequest<GetWithdrawalRequestsResponse>
    {
        public string? StatusFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class WithdrawalRequestDto
    {
        public string Id { get; set; } = string.Empty;
        public string WalletId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewedBy { get; set; }
        public string? Notes { get; set; }
    }

    public class GetWithdrawalRequestsResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public List<WithdrawalRequestDto> Requests { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
