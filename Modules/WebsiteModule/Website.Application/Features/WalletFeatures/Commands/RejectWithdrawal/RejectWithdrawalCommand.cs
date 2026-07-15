using MediatR;

namespace Website.Application.Features.WalletFeatures.Commands.RejectWithdrawal
{
    public class RejectWithdrawalCommand : IRequest<RejectWithdrawalResponse>
    {
        public Guid WithdrawalRequestId { get; set; }
        public string ReviewedBy { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class RejectWithdrawalResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
