using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Application.Posting.Requests;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Enums;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Vouchers.Commands.PostVoucher
{
    public class PostVoucherCommandHandler : IRequestHandler<PostVoucherCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostingService _postingService;
        private readonly IBudgetControlService _budgetControl;

        public PostVoucherCommandHandler(
            IUnitOfWork unitOfWork,
            IPostingService postingService,
            IBudgetControlService budgetControl)
        {
            _unitOfWork     = unitOfWork;
            _postingService = postingService;
            _budgetControl  = budgetControl;
        }

        public async Task<Result<int>> Handle(PostVoucherCommand request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ExecuteTransactionAsync<Result<int>>(async () =>
            {
                var voucher = await _unitOfWork.Vouchers.GetByIdAsync(request.Id);
                if (voucher == null)
                    return Result<int>.Failure("Voucher not found.");

                if (voucher.Status == VoucherStatus.Posted)
                    return Result<int>.Failure("Voucher is already posted.");

                if (voucher.Status != VoucherStatus.Approved)
                    return Result<int>.Failure("Voucher must be Approved before posting.");

                // ── Budget Control re-check inside the transaction ──────────────────
                // Load lines (VoucherLines is a navigation property already included)
                var voucherLines = await _unitOfWork.VoucherLines
                    .FindAsync(l => l.VoucherId == voucher.Id);

                var debitItems = voucherLines
                    .Where(l => l.Debit > 0)
                    .GroupBy(l => new { l.AccountId, l.CostCenterId })
                    .Select(g => new BudgetCheckItem
                    {
                        AccountId    = g.Key.AccountId,
                        CostCenterId = g.Key.CostCenterId,
                        Amount       = g.Sum(l => l.Debit)
                    })
                    .ToList();

                if (debitItems.Count > 0)
                {
                    var budgetResult = await _budgetControl.CheckBudgetListAsync(debitItems, voucher.Date);
                    if (!budgetResult.Success)
                        return Result<int>.Failure(budgetResult.Message);
                }

                // ── Post to General Ledger ─────────────────────────────────────────
                var postingRequest = new PostingRequest
                {
                    SourceType  = SourceType.Voucher,
                    SourceId    = voucher.Id,
                    CurrencyId  = voucher.CurrencyId,
                    Date        = voucher.Date,
                    Description = voucher.Description,
                    Reference   = voucher.Reference
                };

                var postResult = await _postingService.PostAsync(postingRequest);
                await _unitOfWork.SaveChangesAsync();

                if (postResult == null)
                    return Result<int>.Failure("Unexpected null result from posting service.");

                if (!postResult.Success)
                    return Result<int>.Failure(postResult.Message);

                int journalEntryId = postResult.Data.Id;

                voucher.Status       = VoucherStatus.Posted;
                voucher.JournalEntryId = journalEntryId;

                _unitOfWork.Vouchers.Update(voucher);
                await _unitOfWork.SaveChangesAsync();

                return Result<int>.Ok(journalEntryId, "Voucher posted successfully and Journal Entry generated.");
            });
        }
    }
}
