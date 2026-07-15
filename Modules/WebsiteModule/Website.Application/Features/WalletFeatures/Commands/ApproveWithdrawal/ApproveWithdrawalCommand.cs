using MediatR;

namespace Website.Application.Features.WalletFeatures.Commands.ApproveWithdrawal
{
    public class ApproveWithdrawalCommand : IRequest<ApproveWithdrawalResponse>
    {
        public Guid WithdrawalRequestId { get; set; }
        public string ReviewedBy { get; set; } = string.Empty;
    }

    public class ApproveWithdrawalResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
