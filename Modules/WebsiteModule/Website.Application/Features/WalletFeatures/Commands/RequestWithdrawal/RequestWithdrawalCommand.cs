using MediatR;

namespace Website.Application.Features.WalletFeatures.Commands.RequestWithdrawal
{
    public class RequestWithdrawalCommand : IRequest<RequestWithdrawalResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }

    public class RequestWithdrawalResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? WithdrawalRequestId { get; set; }
    }
}
