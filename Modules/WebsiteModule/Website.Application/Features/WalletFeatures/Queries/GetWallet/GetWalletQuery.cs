using MediatR;

namespace Website.Application.Features.WalletFeatures.Queries.GetWallet
{
    public class GetWalletQuery : IRequest<GetWalletResponse>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class GetWalletResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? WalletId { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
