using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.WalletFeatures.Commands.RejectWithdrawal
{
    public class RejectWithdrawalCommandHandler
        : IRequestHandler<RejectWithdrawalCommand, RejectWithdrawalResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RejectWithdrawalCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RejectWithdrawalResponse> Handle(
            RejectWithdrawalCommand command,
            CancellationToken cancellationToken)
        {
            var withdrawalRepo = _unitOfWork.Repository<WithdrawalRequest>();

            var withdrawal = await withdrawalRepo.GetByIdAsync(command.WithdrawalRequestId);
            if (withdrawal == null)
            {
                return new RejectWithdrawalResponse { Success = false, Error = "Withdrawal request not found." };
            }

            if (withdrawal.Status != WithdrawalRequestStatus.Pending)
            {
                return new RejectWithdrawalResponse
                {
                    Success = false,
                    Error = $"Withdrawal request is already {withdrawal.Status}."
                };
            }

            withdrawal.Status = WithdrawalRequestStatus.Rejected;
            withdrawal.ReviewedAt = DateTime.UtcNow;
            withdrawal.ReviewedBy = command.ReviewedBy;
            withdrawal.Notes = command.Notes ?? withdrawal.Notes;
            withdrawalRepo.Update(withdrawal);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RejectWithdrawalResponse { Success = true };
        }
    }
}
