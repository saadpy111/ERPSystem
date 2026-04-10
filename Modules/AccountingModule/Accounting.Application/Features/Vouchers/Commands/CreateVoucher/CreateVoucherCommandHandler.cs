using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Vouchers.Commands.CreateVoucher
{
    public class CreateVoucherCommandHandler : IRequestHandler<CreateVoucherCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateVoucherCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
        {
            var voucherNumber = GenerateVoucherNumber(request.VoucherType);
            
            var voucher = new Voucher
            {
                VoucherNumber = voucherNumber,
                VoucherType = request.VoucherType,
                Date = request.Date,
                Status = VoucherStatus.Draft,
                PartnerId = request.PartnerId,
                PaymentMethod = request.PaymentMethod,
                CurrencyId = request.CurrencyId,
                TotalAmount = request.Lines.Sum(l => l.Debit),
                Reference = request.Reference,
                Description = request.Description
            };

            foreach (var lineDto in request.Lines)
            {
                voucher.Lines.Add(new VoucherLine
                {
                    AccountId = lineDto.AccountId,
                    Debit = lineDto.Debit,
                    Credit = lineDto.Credit,
                    CurrencyId = lineDto.CurrencyId,
                    CostCenterId = lineDto.CostCenterId
                });
            }

            await _unitOfWork.Vouchers.AddAsync(voucher);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Ok(voucher.Id, "Voucher created successfully.");
        }

        private string GenerateVoucherNumber(VoucherType type)
        {
            string prefix = type switch
            {
                VoucherType.Receipt => "RV",
                VoucherType.Payment => "PV",
                VoucherType.Journal => "JV",
                _ => "V"
            };

            return $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
