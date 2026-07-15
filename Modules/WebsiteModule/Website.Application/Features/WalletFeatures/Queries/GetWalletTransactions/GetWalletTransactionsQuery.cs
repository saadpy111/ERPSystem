using MediatR;

namespace Website.Application.Features.WalletFeatures.Queries.GetWalletTransactions
{
    public class GetWalletTransactionsQuery : IRequest<GetWalletTransactionsResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class WalletTransactionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetWalletTransactionsResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public List<WalletTransactionDto> Transactions { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
