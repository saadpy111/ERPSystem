using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Services.Interfaces
{
    /// <summary>
    /// Queries real-time account balances exclusively from Posted General Ledger entries
    /// (JournalEntryLines). This is the financial source of truth for balance calculations.
    ///
    /// This is a Business-layer service. It encapsulates overdraft policies and cash policies.
    /// The Posting Engine does NOT call this service — balance validation is a business rule,
    /// not an accounting constraint.
    /// </summary>
    public interface IAccountBalanceService
    {
    
        Task<decimal> GetAccountBalanceAsync(int accountId, CancellationToken cancellationToken = default);

     
        Task<Result> ValidateSufficientBalanceAsync(
            int glAccountId,
            bool allowNegativeBalance,
            decimal requiredBaseAmount,
            CancellationToken cancellationToken = default);
    }
}
