using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Vouchers.Commands.ApproveVoucher
{
    public class ApproveVoucherCommandHandler : IRequestHandler<ApproveVoucherCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApproveVoucherCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(ApproveVoucherCommand request, CancellationToken cancellationToken)
        {
            var voucher = await _unitOfWork.Vouchers.GetByIdAsync(request.Id);
            if (voucher == null)
                return Result<bool>.Failure("Voucher not found.");

            if (voucher.Status == VoucherStatus.Posted)
                return Result<bool>.Failure("Cannot modify a posted voucher.");

            if (voucher.Status != VoucherStatus.Draft)
                return Result<bool>.Failure($"Cannot approve voucher in {voucher.Status} status.");

            voucher.Status = VoucherStatus.Approved;
            _unitOfWork.Vouchers.Update(voucher);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true, "Voucher approved successfully.");
        }
    }
}
