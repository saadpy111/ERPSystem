using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Application.Posting.Services
{
    public class PostingService : IPostingService
    {
        public const string AlreadyPostedMessage = "Transaction already posted";

        private readonly IEnumerable<IPostingStrategy> _strategies;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<FiscalPeriod> _fiscalPeriodRepository;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IBudgetControlService _budgetControl;
        private readonly ILogger<PostingService> _logger;

        public PostingService(
            IEnumerable<IPostingStrategy> strategies,
            IUnitOfWork unitOfWork,
            IGenericRepository<FiscalPeriod> fiscalPeriodRepository,
            IExchangeRateService exchangeRateService,
            IBudgetControlService budgetControl,
            ILogger<PostingService> logger)
        {
            _strategies             = strategies;
            _unitOfWork             = unitOfWork;
            _fiscalPeriodRepository = fiscalPeriodRepository;
            _exchangeRateService    = exchangeRateService;
            _budgetControl          = budgetControl;
            _logger                 = logger;
        }

        public async Task<Result<JournalEntry>> PostAsync(IPostingRequest request)
        {
            if (request.SourceId <= 0)
                return Result<JournalEntry>.Failure("Invalid SourceId for posting.");

            if (!Enum.IsDefined(typeof(SourceType), request.SourceType))
                return Result<JournalEntry>.Failure("Invalid SourceType.");

            var strategy = _strategies.FirstOrDefault(s => s.CanHandle(request.SourceType));
            if (strategy == null)
                return Result<JournalEntry>.Failure($"No posting strategy found for SourceType {request.SourceType}");

         //   return await _unitOfWork.ExecuteTransactionAsync<Result<JournalEntry>>(async () =>
           // {
                // EARLY idempotency check
                var existingEntries = await _unitOfWork.JournalEntries
                    .FindAsync(e => e.SourceType == request.SourceType && e.SourceId == request.SourceId);
                var alreadyPosted = existingEntries.FirstOrDefault();

                if (alreadyPosted != null)
                {
                    // To handle manual drafts gracefully: only treat as "already posted" if it's external, or if it is posted.
                    if (request.SourceType != SourceType.Manual || alreadyPosted.Status == JournalStatus.Posted)
                    {
                        _logger.LogWarning("Duplicate posting attempt: SourceType={SourceType}, SourceId={SourceId}", request.SourceType, request.SourceId);
                        return Result<JournalEntry>.Ok(alreadyPosted, AlreadyPostedMessage);
                    }
                }

                var existingDraft = request.SourceType == SourceType.Manual
                    ? await _unitOfWork.JournalEntries.GetByIdAsync(request.SourceId)
                    : null;

                List<JournalEntryLine> lines;

                if (existingDraft != null)
                {
                    lines = existingDraft.Lines.ToList();
                }
                else
                {
                    var generateResult = await strategy.GenerateLinesAsync(request);
                    if (!generateResult.Success)
                        return Result<JournalEntry>.Failure(generateResult.Message);
                    
                    lines = generateResult.Data ?? new List<JournalEntryLine>();
                }

                if (lines.Count < 2)
                    return Result<JournalEntry>.Failure("Invalid journal entry lines.");

                var currency = await _unitOfWork.Currencies.GetByIdAsync(request.CurrencyId);
                if (currency == null || !currency.IsActive)
                    return Result<JournalEntry>.Failure("Invalid or inactive currency.");

                var rate = await _exchangeRateService.GetRateAsync(request.CurrencyId, request.Date);

                foreach (var line in lines)
                {
                    decimal foreignAmount = 0;

                    if (line.Debit > 0)
                        foreignAmount = line.Debit;
                    else if (line.Credit > 0)
                        foreignAmount = line.Credit;
                    else
                        return Result<JournalEntry>.Failure("Invalid journal line: both debit and credit are zero.");

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
                    return Result<JournalEntry>.Failure(validationResult.Message);

                try
                {
                    if (existingDraft != null)
                    {
                        existingDraft.Status      = JournalStatus.Posted;
                        existingDraft.PostedAt    = DateTime.UtcNow;
                        existingDraft.CurrencyId  = request.CurrencyId;
                        existingDraft.TotalDebit  = lines.Sum(l => l.Debit);
                        existingDraft.TotalCredit = lines.Sum(l => l.Credit);
                        existingDraft.SourceId    = request.SourceId;
                        existingDraft.SourceType  = request.SourceType;

                        _unitOfWork.JournalEntries.Update(existingDraft);

                    //    await _unitOfWork.SaveChangesAsync();

                        return Result<JournalEntry>.Ok(existingDraft);
                    }
                    else
                    {
                        var activePeriodResult = await GetOpenFiscalPeriodAsync(request.Date);
                        if (!activePeriodResult.Success)
                            return Result<JournalEntry>.Failure(activePeriodResult.Message);

                        var activePeriod = activePeriodResult.Data!;

                        var newEntry = new JournalEntry
                        {
                            JournalNumber = "JE-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                            Date          = request.Date,
                            Reference     = request.Reference,
                            CurrencyId    = request.CurrencyId,
                            SourceType    = request.SourceType,
                            SourceId      = request.SourceId,
                            Description   = request.Description,
                            Status        = JournalStatus.Posted,
                            TotalDebit    = lines.Sum(l => l.Debit),
                            TotalCredit   = lines.Sum(l => l.Credit),
                            PostedAt      = DateTime.UtcNow,
                            FiscalPeriodId = activePeriod.Id,
                            Lines         = lines
                        };

                        await _unitOfWork.JournalEntries.AddAsync(newEntry);
                   //     await _unitOfWork.SaveChangesAsync();

                        return Result<JournalEntry>.Ok(newEntry);
                    }
                }
                catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
                {
                    var postViolationEntries = await _unitOfWork.JournalEntries
                        .FindAsync(e => e.SourceType == request.SourceType && e.SourceId == request.SourceId);
                    var postViolationEntry = postViolationEntries.FirstOrDefault();

                    if (postViolationEntry != null)
                    {
                        _logger.LogWarning("Duplicate posting caught by constraJournalEntry: SourceType={SourceType}, SourceId={SourceId}", request.SourceType, request.SourceId);
                        return Result<JournalEntry>.Ok(postViolationEntry, AlreadyPostedMessage);
                    }
                    throw;
                }
          //  });
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            var innerMsg = ex.InnerException?.Message ?? string.Empty;
            return innerMsg.IndexOf("duplicate", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   innerMsg.IndexOf("unique", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   innerMsg.IndexOf("constraint", StringComparison.OrdinalIgnoreCase) >= 0;
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
