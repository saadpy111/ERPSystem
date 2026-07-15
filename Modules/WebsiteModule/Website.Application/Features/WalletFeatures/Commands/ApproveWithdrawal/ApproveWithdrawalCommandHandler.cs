using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.WalletFeatures.Commands.ApproveWithdrawal
{
    public class ApproveWithdrawalCommandHandler
        : IRequestHandler<ApproveWithdrawalCommand, ApproveWithdrawalResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public ApproveWithdrawalCommandHandler(
            IUnitOfWork unitOfWork,
            ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<ApproveWithdrawalResponse> Handle(
            ApproveWithdrawalCommand command,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId();
            var withdrawalRepo = _unitOfWork.Repository<WithdrawalRequest>();
            var walletRepo = _unitOfWork.Repository<Wallet>();
            var walletTxRepo = _unitOfWork.Repository<WalletTransaction>();

            var withdrawal = await withdrawalRepo.GetByIdAsync(command.WithdrawalRequestId);
            if (withdrawal == null)
            {
                return new ApproveWithdrawalResponse { Success = false, Error = "Withdrawal request not found." };
            }

            if (withdrawal.Status != WithdrawalRequestStatus.Pending)
            {
                return new ApproveWithdrawalResponse
                {
                    Success = false,
                    Error = $"Withdrawal request is already {withdrawal.Status}."
                };
            }

            var wallet = await _unitOfWork.Repository<Wallet>().GetByIdAsync(withdrawal.WalletId);
            if (wallet == null)
            {
                return new ApproveWithdrawalResponse { Success = false, Error = "Wallet not found." };
            }

            if (withdrawal.Amount > wallet.CurrentBalance)
            {
                withdrawal.Status = WithdrawalRequestStatus.Rejected;
                withdrawal.ReviewedAt = DateTime.UtcNow;
                withdrawal.ReviewedBy = command.ReviewedBy;
                withdrawal.Notes = "Insufficient balance at time of approval.";
                withdrawalRepo.Update(withdrawal);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new ApproveWithdrawalResponse
                {
                    Success = false,
                    Error = "Insufficient balance. Withdrawal request rejected."
                };
            }

            var balanceBefore = wallet.CurrentBalance;
            wallet.CurrentBalance -= withdrawal.Amount;
            walletRepo.Update(wallet);

            var walletTx = new WalletTransaction
            {
                WalletId = wallet.Id,
                Type = WalletTransactionType.Debit,
                Amount = withdrawal.Amount,
                BalanceBefore = balanceBefore,
                BalanceAfter = wallet.CurrentBalance,
                Reference = $"Withdrawal:{withdrawal.Id}",
                Description = $"Withdrawal approved by {command.ReviewedBy}",
                TenantId = tenantId
            };
            await walletTxRepo.AddAsync(walletTx);

            withdrawal.Status = WithdrawalRequestStatus.Approved;
            withdrawal.ReviewedAt = DateTime.UtcNow;
            withdrawal.ReviewedBy = command.ReviewedBy;
            withdrawalRepo.Update(withdrawal);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ApproveWithdrawalResponse { Success = true };
        }
    }
}
