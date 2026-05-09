using Accounting.Application.Common.Constants;
using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Strategies
{
    /// <summary>
    /// Generates GL lines for a Receivable Payment (collecting from a customer).
    ///
    /// Currency ownership rule:
    ///   ReceivablePayment.ExchangeRate is locked at payment creation.
    ///   This strategy reads those values and produces fully-computed lines.
    ///   The Posting Engine never recalculates exchange rates.
    ///
    /// Accounting entry:
    ///   Dr Cash Account         (asset increases)
    ///   Cr Accounts Receivable  (reduces receivable)
    /// </summary>
    public class ReceivablePaymentPostingStrategy : IPostingStrategy
    {
        private readonly IAccountingMappingService _mappingService;
        private readonly IUnitOfWork _uow;

        public ReceivablePaymentPostingStrategy(IAccountingMappingService mappingService, IUnitOfWork uow)
        {
            _mappingService = mappingService;
            _uow = uow;
        }

        public bool CanHandle(SourceType type) => type == SourceType.ReceivablePayment;

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var payment = await _uow.ReceivablePayments.GetByIdAsync(request.SourceId);

            if (payment == null)
                return Result<List<JournalEntryLine>>.Failure(
                    $"Receivable Payment with ID {request.SourceId} not found.");

            if (payment.Amount <= 0)
                return Result<List<JournalEntryLine>>.Failure(
                    "Invalid receivable payment amount.");

            var cashAccount = await _uow.CashAccounts.GetByIdAsync(payment.CashAccountId);
            if (cashAccount == null || cashAccount.AccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure(
                    $"Invalid Cash Account setup for payment ID {request.SourceId}.");

            var arAccountId = await _mappingService.GetAccountIdAsync(
                SourceType.ReceivablePayment, MappingKeys.AccountsReceivable);

            if (arAccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure(
                    "Missing account mapping for Accounts Receivable.");

            var receivable = await _uow.Receivables.GetByIdAsync(payment.ReceivableId);
            int? partnerId = receivable?.PartnerId;

            // Use base-currency amount locked at payment creation.
            // If BaseAmount is 0 (legacy data or domestic), fall back to Amount.
            decimal baseAmount    = payment.BaseAmount > 0 ? payment.BaseAmount : payment.Amount;
            decimal foreignAmount = payment.ForeignAmount > 0 ? payment.ForeignAmount : payment.Amount;
            decimal exchangeRate  = payment.ExchangeRate > 0 ? payment.ExchangeRate : 1m;
            int     currencyId    = payment.CurrencyId > 0 ? payment.CurrencyId : request.CurrencyId;

            var lines = new List<JournalEntryLine>
            {
                // Dr Cash Account
                new JournalEntryLine
                {
                    AccountId     = cashAccount.AccountId,
                    Debit         = baseAmount,
                    Credit        = 0,
                    ForeignAmount = foreignAmount,
                    ExchangeRate  = exchangeRate,
                    BaseAmount    = baseAmount,
                    CurrencyId    = currencyId,
                    PartnerId     = partnerId,
                    Description   = payment.Description ?? "Cash Receipt for Receivable"
                },
                // Cr Accounts Receivable
                new JournalEntryLine
                {
                    AccountId     = arAccountId,
                    Debit         = 0,
                    Credit        = baseAmount,
                    ForeignAmount = foreignAmount,
                    ExchangeRate  = exchangeRate,
                    BaseAmount    = baseAmount,
                    CurrencyId    = currencyId,
                    PartnerId     = partnerId,
                    Description   = payment.Description ?? "Accounts Receivable Payment"
                }
            };

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
