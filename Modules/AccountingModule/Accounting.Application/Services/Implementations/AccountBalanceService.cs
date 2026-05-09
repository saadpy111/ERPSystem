using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Services.Implementations
{

    public class AccountBalanceService : IAccountBalanceService
    {
        private readonly IAccountingDbContext _context;

        public AccountBalanceService(IAccountingDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<decimal> GetAccountBalanceAsync(
            int accountId,
            CancellationToken cancellationToken = default)
        {

            var totalDebit = await _context.JournalEntryLines
                .Where(l =>
                    l.AccountId == accountId &&
                    l.JournalEntry.Status == JournalStatus.Posted &&
                    !l.IsDeleted)
                .SumAsync(l => l.Debit, cancellationToken);

            var totalCredit = await _context.JournalEntryLines
                .Where(l =>
                    l.AccountId == accountId &&
                    l.JournalEntry.Status == JournalStatus.Posted &&
                    !l.IsDeleted)
                .SumAsync(l => l.Credit, cancellationToken);

            return totalDebit - totalCredit;
        }

        /// <inheritdoc />
        public async Task<Result> ValidateSufficientBalanceAsync(
            int glAccountId,
            bool allowNegativeBalance,
            decimal requiredBaseAmount,
            CancellationToken cancellationToken = default)
        {
            if (allowNegativeBalance)
                return Result.Ok();

            if (requiredBaseAmount <= 0)
                return Result.Ok();

            var currentBalance = await GetAccountBalanceAsync(glAccountId, cancellationToken);

            if (currentBalance < requiredBaseAmount)
                return Result.Failure(
                    $"Insufficient balance. Available: {currentBalance:F2}, Required: {requiredBaseAmount:F2}. " +
                    $"Account does not allow negative balances.");

            return Result.Ok();
        }
    }
}
