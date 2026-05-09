using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Posting.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Strategies
{
    /// <summary>
    /// Generates GL journal lines for Cash Receipt and Cash Payment transactions.
    ///
    /// Currency ownership rule:
    ///   The CashTransaction entity owns the exchange rate — it was locked at the moment
    ///   the business transaction was created. This strategy reads those pre-computed values
    ///   and populates ForeignAmount, ExchangeRate, and BaseAmount on each JournalEntryLine.
    ///   The Posting Engine NEVER recalculates or overwrites exchange rates.
    ///
    ///   BaseAmount = ForeignAmount × ExchangeRate  (already computed on CashTransaction)
    ///   Debit / Credit on the line = BaseAmount (base currency only stored in the ledger)
    /// </summary>
    public class CashPostingStrategy : IPostingStrategy
    {
        private readonly IAccountingDbContext _context;

        public CashPostingStrategy(IAccountingDbContext context)
        {
            _context = context;
        }

        public bool CanHandle(SourceType type) =>
            type == SourceType.CashReceipt || type == SourceType.CashPayment;

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var transaction = await _context.CashTransactions
                .Include(t => t.CashAccount)
                .FirstOrDefaultAsync(t => t.Id == request.SourceId);

            if (transaction == null)
                return Result<List<JournalEntryLine>>.Failure(
                    "Cash transaction not found for posting.");

            if (transaction.CashAccount == null || transaction.CashAccount.AccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure(
                    "Cash Account is missing or lacks a mapped GL AccountId.");

            if (transaction.OffsetAccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure(
                    "An OffsetAccountId is strictly required to post a cash transaction.");

            if (transaction.Amount <= 0)
                return Result<List<JournalEntryLine>>.Failure(
                    "Cash transaction must have an amount greater than zero.");

            // BaseAmount was locked at transaction creation: BaseAmount = Amount × ExchangeRate
            // We use BaseAmount as the Debit/Credit value on the ledger (base currency).
            decimal foreignAmount = transaction.Amount;
            decimal exchangeRate  = transaction.ExchangeRate;
            decimal baseAmount    = transaction.BaseAmount;

            var lines = new List<JournalEntryLine>();

            if (transaction.Type == CashTransactionType.Receipt)
            {
                // Cash Receipt:
                //   Dr Cash Account     (asset increases)
                //   Cr Offset Account   (liability/revenue/AR decreases or recognition)
                lines.Add(new JournalEntryLine
                {
                    AccountId     = transaction.CashAccount.AccountId,
                    Debit         = baseAmount,
                    Credit        = 0,
                    ForeignAmount = foreignAmount,
                    ExchangeRate  = exchangeRate,
                    BaseAmount    = baseAmount,
                    CurrencyId    = transaction.CurrencyId,
                    PartnerId     = transaction.PartnerId,
                    Description   = transaction.Description
                });

                lines.Add(new JournalEntryLine
                {
                    AccountId     = transaction.OffsetAccountId,
                    Debit         = 0,
                    Credit        = baseAmount,
                    ForeignAmount = foreignAmount,
                    ExchangeRate  = exchangeRate,
                    BaseAmount    = baseAmount,
                    CurrencyId    = transaction.CurrencyId,
                    PartnerId     = transaction.PartnerId,
                    Description   = transaction.Description
                });
            }
            else // CashTransactionType.Payment
            {
                // Cash Payment:
                //   Dr Offset Account   (expense/AP/asset increases)
                //   Cr Cash Account     (asset decreases)
                lines.Add(new JournalEntryLine
                {
                    AccountId     = transaction.OffsetAccountId,
                    Debit         = baseAmount,
                    Credit        = 0,
                    ForeignAmount = foreignAmount,
                    ExchangeRate  = exchangeRate,
                    BaseAmount    = baseAmount,
                    CurrencyId    = transaction.CurrencyId,
                    PartnerId     = transaction.PartnerId,
                    Description   = transaction.Description
                });

                lines.Add(new JournalEntryLine
                {
                    AccountId     = transaction.CashAccount.AccountId,
                    Debit         = 0,
                    Credit        = baseAmount,
                    ForeignAmount = foreignAmount,
                    ExchangeRate  = exchangeRate,
                    BaseAmount    = baseAmount,
                    CurrencyId    = transaction.CurrencyId,
                    PartnerId     = transaction.PartnerId,
                    Description   = transaction.Description
                });
            }

            if (lines.Sum(l => l.Debit) != lines.Sum(l => l.Credit))
                return Result<List<JournalEntryLine>>.Failure(
                    "Generated entry lines are unbalanced — double-entry principle violated.");

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
