using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Application.Posting.Requests;
using Accounting.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Vouchers.Commands.PostVoucher
{
    public class PostVoucherCommandHandler : IRequestHandler<PostVoucherCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostingService _postingService;

        public PostVoucherCommandHandler(IUnitOfWork unitOfWork, IPostingService postingService)
        {
            _unitOfWork = unitOfWork;
            _postingService = postingService;
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

                var postingRequest = new PostingRequest
                {
                    SourceType = SourceType.Voucher,
                    SourceId = voucher.Id,
                    CurrencyId = voucher.CurrencyId,
                    Date = voucher.Date,
                    Description = voucher.Description,
                    Reference = voucher.Reference
                };

                var postResult = await _postingService.PostAsync(postingRequest);

                if (postResult == null)
                    return Result<int>.Failure("Unexpected null result from posting service.");

                if (!postResult.Success)
                    return Result<int>.Failure(postResult.Message);

                int journalEntryId = postResult.Data;

                voucher.Status = VoucherStatus.Posted;
                voucher.JournalEntryId = journalEntryId;
                
                _unitOfWork.Vouchers.Update(voucher);
                await _unitOfWork.SaveChangesAsync();

                return Result<int>.Ok(journalEntryId, "Voucher posted successfully and Journal Entry generated.");
            });
        }
    }
}
