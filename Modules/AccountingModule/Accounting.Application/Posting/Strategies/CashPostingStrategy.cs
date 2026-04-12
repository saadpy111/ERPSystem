using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Posting.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;

namespace Accounting.Application.Posting.Strategies
{
    public class CashPostingStrategy : IPostingStrategy
    {
        private readonly IAccountingDbContext _context;

        public CashPostingStrategy(IAccountingDbContext context)
        {
            _context = context;
        }

        public bool CanHandle(SourceType type) => type == SourceType.CashReceipt || type == SourceType.CashPayment;

        public async Task<Result<List<JournalEntryLine>>> GenerateLinesAsync(IPostingRequest request)
        {
            var transaction = await _context.CashTransactions
                .Include(t => t.CashAccount)
                .FirstOrDefaultAsync(t => t.Id == request.SourceId);

            if (transaction == null)
                return Result<List<JournalEntryLine>>.Failure("Cash transaction not found for posting.");

            if (transaction.CashAccount == null || transaction.CashAccount.AccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure("Cash Account is missing or lacks a mapped GL AccountId.");

            if (transaction.OffsetAccountId <= 0)
                return Result<List<JournalEntryLine>>.Failure("An OffsetAccountId is strictly required to post a cash transaction.");

            if (transaction.Amount <= 0)
                return Result<List<JournalEntryLine>>.Failure("Cash transaction must have an amount greater than zero.");

            var lines = new List<JournalEntryLine>();

            if (transaction.Type == CashTransactionType.Receipt)
            {
                // Debit Cash Account
                lines.Add(new JournalEntryLine
                {
                    AccountId = transaction.CashAccount.AccountId,
                    Debit = transaction.Amount,
                    Credit = 0,
                    PartnerId = transaction.PartnerId,
                    Description = transaction.Description
                });

                // Credit Offset Account
                lines.Add(new JournalEntryLine
                {
                    AccountId = transaction.OffsetAccountId,
                    Debit = 0,
                    Credit = transaction.Amount,
                    PartnerId = transaction.PartnerId,
                    Description = transaction.Description
                });
            }
            else // Payment
            {
                // Debit Offset Account
                lines.Add(new JournalEntryLine
                {
                    AccountId = transaction.OffsetAccountId,
                    Debit = transaction.Amount,
                    Credit = 0,
                    PartnerId = transaction.PartnerId,
                    Description = transaction.Description
                });

                // Credit Cash Account
                lines.Add(new JournalEntryLine
                {
                    AccountId = transaction.CashAccount.AccountId,
                    Debit = 0,
                    Credit = transaction.Amount,
                    PartnerId = transaction.PartnerId,
                    Description = transaction.Description
                });
            }

            if (lines.Count < 2)
                return Result<List<JournalEntryLine>>.Failure("Failed to generate double-entry lines. Minimum 2 lines required.");

            if (lines.Sum(l => l.Debit) != lines.Sum(l => l.Credit))
                return Result<List<JournalEntryLine>>.Failure("Generated entry lines are unequal, breaking double-entry principles.");

            return Result<List<JournalEntryLine>>.Ok(lines);
        }
    }
}
