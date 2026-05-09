using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Strategies
{
    /// <summary>
    /// Generates GL lines from a Journal Voucher.
    ///
    /// Voucher types and their behaviour:
    ///   - Journal Voucher: user enters Debit/Credit + ForeignAmount + ExchangeRate manually.
    ///     This strategy passes them through directly. The user is fully responsible for
    ///     double-entry correctness; the Posting Engine validates it.
    ///
    /// Currency ownership rule:
    ///   VoucherLine owns ForeignAmount and ExchangeRate (locked when the voucher was created).
    ///   Debit/Credit on VoucherLine MUST already be in Base Currency (BaseAmount = Foreign × Rate).
    ///   This strategy passes values through — the Posting Engine never recalculates rates.
    /// </summary>
    public class VoucherPostingStrategy : IPostingStrategy
    {
        private readonly IUnitOfWork _unitOfWork;

        public VoucherPostingStrategy(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CanHandle(SourceType type) => type == SourceType.Voucher;

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var voucher = await _unitOfWork.Vouchers.GetByIdWithLinesAsync(request.SourceId);

            if (voucher == null)
                return Result<List<JournalEntryLine>>.Failure(
                    $"Voucher with ID {request.SourceId} not found.");

            if (!voucher.Lines.Any())
                return Result<List<JournalEntryLine>>.Failure(
                    $"Voucher {request.SourceId} has no lines.");

            var lines = voucher.Lines.Select(line => new JournalEntryLine
            {
                AccountId     = line.AccountId,
                Debit         = line.Debit,         // already in Base Currency
                Credit        = line.Credit,        // already in Base Currency
                CostCenterId  = line.CostCenterId,
                CurrencyId    = line.CurrencyId,
                ForeignAmount = line.ForeignAmount, // pass through — locked at voucher creation
                ExchangeRate  = line.ExchangeRate,  // pass through — never recalculated
                BaseAmount    = line.BaseAmount ?? (line.Debit > 0 ? line.Debit : line.Credit),
                Description   = voucher.Description
            }).ToList();

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
