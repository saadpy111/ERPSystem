using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Services
{
    public class PostingService : IPostingService
    {
        private readonly IEnumerable<IPostingStrategy> _strategies;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<FiscalPeriod> _fiscalPeriodRepository;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IBudgetControlService _budgetControl;

        public PostingService(
            IEnumerable<IPostingStrategy> strategies,
            IUnitOfWork unitOfWork,
            IGenericRepository<FiscalPeriod> fiscalPeriodRepository,
            IExchangeRateService exchangeRateService,
            IBudgetControlService budgetControl)
        {
            _strategies             = strategies;
            _unitOfWork             = unitOfWork;
            _fiscalPeriodRepository = fiscalPeriodRepository;
            _exchangeRateService    = exchangeRateService;
            _budgetControl          = budgetControl;
        }

        public async Task<Result<int>> PostAsync(IPostingRequest request)
        {
            var strategy = _strategies.FirstOrDefault(s => s.CanHandle(request.SourceType));
            if (strategy == null)
                return Result<int>.Failure($"No posting strategy found for SourceType {request.SourceType}");

            var existingEntry = request.SourceType == SourceType.Manual
                ? await _unitOfWork.JournalEntries.GetByIdAsync(request.SourceId)
                : null;

            var lines = existingEntry != null
                ? existingEntry.Lines.ToList()
                : await strategy.GenerateLinesAsync(request);

            if (lines == null || lines.Count < 2)
                return Result<int>.Failure("Invalid journal entry lines.");

            var currency = await _unitOfWork.Currencies.GetByIdAsync(request.CurrencyId);
            if (currency == null || !currency.IsActive)
                return Result<int>.Failure("Invalid or inactive currency.");

            var rate = await _exchangeRateService.GetRateAsync(request.CurrencyId, request.Date);

            foreach (var line in lines)
            {
                decimal foreignAmount = 0;

                if (line.Debit > 0)
                    foreignAmount = line.Debit;
                else if (line.Credit > 0)
                    foreignAmount = line.Credit;
                else
                    return Result<int>.Failure("Invalid journal line: both debit and credit are zero.");

                line.CurrencyId    = request.CurrencyId;
                line.ExchangeRate  = rate;
                line.ForeignAmount = foreignAmount;
                line.BaseAmount    = foreignAmount * rate;

                if (line.Debit > 0)
                {
                    line.Debit  = line.BaseAmount;
                    line.Credit = 0;
                }
                else
                {
                    line.Credit = line.BaseAmount;
                    line.Debit  = 0;
                }
            }

            var validationResult = await ValidateJournalEntryAsync(lines, request.Date);
            if (!validationResult.Success)
                return Result<int>.Failure(validationResult.Message);

            if (existingEntry != null)
            {
                existingEntry.Status      = JournalStatus.Posted;
                existingEntry.PostedAt    = DateTime.UtcNow;
                existingEntry.CurrencyId  = request.CurrencyId;
                existingEntry.TotalDebit  = lines.Sum(l => l.Debit);
                existingEntry.TotalCredit = lines.Sum(l => l.Credit);

                _unitOfWork.JournalEntries.Update(existingEntry);

                await _unitOfWork.SaveChangesAsync();

                return Result<int>.Ok(existingEntry.Id);
            }
            else
            {
                var activePeriodResult = await GetOpenFiscalPeriodAsync(request.Date);
                if (!activePeriodResult.Success)
                    return Result<int>.Failure(activePeriodResult.Message);

                var activePeriod = activePeriodResult.Data!;

                var newEntry = new JournalEntry
                {
                    JournalNumber = "JE-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Date          = request.Date,
                    Reference     = request.Reference,
                    CurrencyId    = request.CurrencyId,
                    SourceType    = request.SourceType,
                    Description   = request.Description,
                    Status        = JournalStatus.Posted,
                    TotalDebit    = lines.Sum(l => l.Debit),
                    TotalCredit   = lines.Sum(l => l.Credit),
                    PostedAt      = DateTime.UtcNow,
                    FiscalPeriodId = activePeriod.Id,
                    Lines         = lines
                };

                await _unitOfWork.JournalEntries.AddAsync(newEntry);
                await _unitOfWork.SaveChangesAsync();

                return Result<int>.Ok(newEntry.Id);
            }
        }

        private async Task<Result> ValidateJournalEntryAsync(List<JournalEntryLine> lines, DateTime date)
        {
            var totalDebit  = Math.Round(lines.Sum(l => l.Debit),  6);
            var totalCredit = Math.Round(lines.Sum(l => l.Credit), 6);

            if (totalDebit != totalCredit)
                return Result.Failure($"Unbalanced entry: Debit ({totalDebit}) != Credit ({totalCredit})");

            if (lines.Any(l => l.Debit == 0 && l.Credit == 0))
                return Result.Failure("Zero lines are not allowed.");

            var accountIds  = lines.Select(l => l.AccountId).Distinct().ToList();
            var leafAccounts = await _unitOfWork.Accounts.GetLeafAccountsAsync();
            var leafIds     = leafAccounts.Select(a => a.Id).ToList();

            foreach (var accId in accountIds)
            {
                if (!leafIds.Contains(accId))
                    return Result.Failure($"Account {accId} is not a leaf account.");
            }

            var periodRes = await GetOpenFiscalPeriodAsync(date);
            if (!periodRes.Success)
                return Result.Failure(periodRes.Message);

            // ── Final budget revalidation inside the transaction (concurrency safety) ──
            var debitItems = lines
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
                var budgetResult = await _budgetControl.CheckBudgetListAsync(debitItems, date);
                if (!budgetResult.Success)
                    return Result.Failure(budgetResult.Message);
            }

            return Result.Ok();
        }

        private async Task<Result<FiscalPeriod>> GetOpenFiscalPeriodAsync(DateTime date)
        {
            var openPeriods = await _fiscalPeriodRepository
                .FindAsync(p => !p.IsClosed && date >= p.StartDate && date <= p.EndDate);

            var activePeriod = openPeriods.FirstOrDefault();

            if (activePeriod == null)
                return Result<FiscalPeriod>.Failure("No open fiscal period found for the specified date.");

            return Result<FiscalPeriod>.Ok(activePeriod);
        }
    }
}
