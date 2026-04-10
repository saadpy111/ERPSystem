using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Vouchers.Commands.CreateVoucher
{
    public class CreateVoucherCommandHandler : IRequestHandler<CreateVoucherCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBudgetControlService _budgetControl;

        public CreateVoucherCommandHandler(IUnitOfWork unitOfWork, IBudgetControlService budgetControl)
        {
            _unitOfWork = unitOfWork;
            _budgetControl = budgetControl;
        }

        public async Task<Result<int>> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
        {
            // ── Budget Control (Debit lines only, aggregated per Account+CostCenter) ──
            var debitItems = request.Lines
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
                var budgetResult = await _budgetControl.CheckBudgetListAsync(debitItems, request.Date);

                if (!budgetResult.Success)
                    return Result<int>.Failure(budgetResult.Message);

                // Warning is preserved on the returned result below
                if (budgetResult.HasWarning)
                {
                    // Still create the voucher, but surface the warning to the caller
                    var voucherId = await PersistVoucherAsync(request);
                    return Result<int>.SuccessWithWarning(voucherId, budgetResult.Message);
                }
            }

            var id = await PersistVoucherAsync(request);
            return Result<int>.Ok(id, "Voucher created successfully.");
        }

        // ── Persistence ────────────────────────────────────────────────────────────

        private async Task<int> PersistVoucherAsync(CreateVoucherCommand request)
        {
            var voucherNumber = GenerateVoucherNumber(request.VoucherType);

            var voucher = new Voucher
            {
                VoucherNumber = voucherNumber,
                VoucherType   = request.VoucherType,
                Date          = request.Date,
                Status        = VoucherStatus.Draft,
                PartnerId     = request.PartnerId,
                PaymentMethod = request.PaymentMethod,
                CurrencyId    = request.CurrencyId,
                TotalAmount   = request.Lines.Sum(l => l.Debit),
                Reference     = request.Reference,
                Description   = request.Description
            };

            foreach (var lineDto in request.Lines)
            {
                voucher.Lines.Add(new VoucherLine
                {
                    AccountId    = lineDto.AccountId,
                    Debit        = lineDto.Debit,
                    Credit       = lineDto.Credit,
                    CurrencyId   = lineDto.CurrencyId,
                    CostCenterId = lineDto.CostCenterId
                });
            }

            await _unitOfWork.Vouchers.AddAsync(voucher);
            await _unitOfWork.SaveChangesAsync();

            return voucher.Id;
        }

        private string GenerateVoucherNumber(VoucherType type)
        {
            string prefix = type switch
            {
                VoucherType.Receipt => "RV",
                VoucherType.Payment => "PV",
                VoucherType.Journal => "JV",
                _                  => "V"
            };

            return $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
