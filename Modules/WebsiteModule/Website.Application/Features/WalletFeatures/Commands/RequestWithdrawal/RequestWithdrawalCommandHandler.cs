using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.WalletFeatures.Commands.RequestWithdrawal
{
    public class RequestWithdrawalCommandHandler
        : IRequestHandler<RequestWithdrawalCommand, RequestWithdrawalResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public RequestWithdrawalCommandHandler(
            IUnitOfWork unitOfWork,
            ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<RequestWithdrawalResponse> Handle(
            RequestWithdrawalCommand command,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId();
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                return new RequestWithdrawalResponse { Success = false, Error = "Tenant not resolved." };
            }

            var wallets = await _unitOfWork.Repository<Wallet>()
                .GetAllAsync(w => w.TenantId == tenantId);
            var wallet = wallets.FirstOrDefault();

            if (wallet == null)
            {
                return new RequestWithdrawalResponse
                {
                    Success = false,
                    Error = "No wallet found for this merchant."
                };
            }

            if (command.Amount <= 0)
            {
                return new RequestWithdrawalResponse
                {
                    Success = false,
                    Error = "Withdrawal amount must be greater than zero."
                };
            }

            if (command.Amount > wallet.CurrentBalance)
            {
                return new RequestWithdrawalResponse
                {
                    Success = false,
                    Error = "Insufficient balance."
                };
            }

            var withdrawal = new WithdrawalRequest
            {
                WalletId = wallet.Id,
                Amount = command.Amount,
                Status = WithdrawalRequestStatus.Pending,
                RequestedAt = DateTime.UtcNow,
                Notes = command.Notes,
                TenantId = tenantId
            };

            await _unitOfWork.Repository<WithdrawalRequest>().AddAsync(withdrawal);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RequestWithdrawalResponse
            {
                Success = true,
                WithdrawalRequestId = withdrawal.Id.ToString()
            };
        }
    }
}
