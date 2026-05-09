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
        private readonly IBudgetControlService _budgetControl;
        private readonly ILogger<PostingService> _logger;

        public PostingService(
            IEnumerable<IPostingStrategy> strategies,
            IUnitOfWork unitOfWork,
            IGenericRepository<FiscalPeriod> fiscalPeriodRepository,
            IBudgetControlService budgetControl,
            ILogger<PostingService> logger)
        {
            _strategies = strategies;
            _unitOfWork = unitOfWork;
            _fiscalPeriodRepository = fiscalPeriodRepository;
            _budgetControl = budgetControl;
            _logger = logger;
        }

        public async Task<Result<JournalEntry>> PostAsync(IPostingRequest request)
        {
            // ── Guard: source type & id ──────────────────────────────────────────────
            if (!Enum.IsDefined(typeof(SourceType), request.SourceType))
                return Result<JournalEntry>.Failure("Invalid SourceType.");

            if (request.SourceType != SourceType.Manual && request.SourceId <= 0)
                return Result<JournalEntry>.Failure("Invalid SourceId for posting.");

            // ── Idempotency: non-manual sources ─────────────────────────────────────
            // Manual journal entries (Journal Voucher) bypass idempotency — each post
            // of a manual draft is intentional. All other source types are idempotent.
            if (request.SourceType != SourceType.Manual)
            {
                var existingEntries = await _unitOfWork.JournalEntries
                    .FindAsync(e =>
                        e.SourceType == request.SourceType &&
                        e.SourceId == request.SourceId);

                var alreadyPosted = existingEntries.FirstOrDefault();

                if (alreadyPosted != null)
                {
                    _logger.LogWarning(
                        "Duplicate posting attempt: SourceType={SourceType}, SourceId={SourceId}",
                        request.SourceType,
                        request.SourceId);

                    return Result<JournalEntry>.Ok(alreadyPosted, AlreadyPostedMessage);
                }
            }

            // ── Resolve strategy ────────────────────────────────────────────────────
            var strategy = _strategies.FirstOrDefault(s => s.CanHandle(request.SourceType));

            if (strategy == null)
                return Result<JournalEntry>.Failure(
                    $"No posting strategy found for SourceType {request.SourceType}");

            // ── For manual drafts: load existing draft lines directly ────────────────
            // For all other sources: delegate line generation to the posting strategy.
            List<JournalEntryLine> lines;

            if (request.SourceType == SourceType.Manual && request.SourceId > 0)
            {
                var existingDraft = await _unitOfWork.JournalEntries
                    .GetByIdWithLinesAsync(request.SourceId);

                if (existingDraft == null)
                    return Result<JournalEntry>.Failure(
                        $"Manual journal draft with ID {request.SourceId} not found.");

                if (existingDraft.Status == JournalStatus.Posted)
                    return Result<JournalEntry>.Ok(existingDraft, AlreadyPostedMessage);

                lines = existingDraft.Lines.ToList();

                // ── Validate & persist the draft ─────────────────────────────────
                var draftValidation = await ValidateJournalEntryAsync(lines, request.Date);
                if (!draftValidation.Success)
                    return Result<JournalEntry>.Failure(draftValidation.Message);

                var activePeriodForDraft = await GetOpenFiscalPeriodAsync(request.Date);
                if (!activePeriodForDraft.Success)
                    return Result<JournalEntry>.Failure(activePeriodForDraft.Message);

                existingDraft.Status = JournalStatus.Posted;
                existingDraft.PostedAt = DateTime.UtcNow;
                existingDraft.PostedBy = request.PostedBy;
                existingDraft.TotalDebit = lines.Sum(l => l.Debit);
                existingDraft.TotalCredit = lines.Sum(l => l.Credit);
                existingDraft.FiscalPeriodId = activePeriodForDraft.Data!.Id;

                _unitOfWork.JournalEntries.Update(existingDraft);
                await _unitOfWork.SaveChangesAsync();

                return Result<JournalEntry>.Ok(existingDraft);
            }

            // ── Generate lines via strategy ──────────────────────────────────────────
            // Strategies return fully computed lines with:
            //   - Debit / Credit in BASE CURRENCY (already converted using the locked rate)
            //   - ForeignAmount  = original foreign-currency amount
            //   - ExchangeRate   = the rate locked at transaction creation
            //   - BaseAmount     = ForeignAmount × ExchangeRate
            // The Posting Engine NEVER recalculates exchange rates.
            var generateResult = await strategy.GenerateLinesAsync(request);

            if (!generateResult.Success)
                return Result<JournalEntry>.Failure(generateResult.Message);

            lines = generateResult.Data ?? new List<JournalEntryLine>();

            // ── Validate ledger integrity ────────────────────────────────────────────
            var validationResult = await ValidateJournalEntryAsync(lines, request.Date);

            if (!validationResult.Success)
                return Result<JournalEntry>.Failure(validationResult.Message);

            // ── Validate currency ────────────────────────────────────────────────────
            var currency = await _unitOfWork.Currencies.GetByIdAsync(request.CurrencyId);

            if (currency == null || !currency.IsActive)
                return Result<JournalEntry>.Failure("Invalid or inactive currency.");

            // ── Get open fiscal period ───────────────────────────────────────────────
            var activePeriodResult = await GetOpenFiscalPeriodAsync(request.Date);

            if (!activePeriodResult.Success)
                return Result<JournalEntry>.Failure(activePeriodResult.Message);

            var activePeriod = activePeriodResult.Data!;

            // ── Persist atomically ───────────────────────────────────────────────────
            try
            {
                var newEntry = new JournalEntry
                {
                    JournalNumber = "JE-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Date = request.Date,
                    Reference = request.Reference,
                    CurrencyId = request.CurrencyId,
                    SourceType = request.SourceType,
                    SourceId = request.SourceId,
                    Description = request.Description,
                    Status = JournalStatus.Posted,
                    TotalDebit = lines.Sum(l => l.Debit),
                    TotalCredit = lines.Sum(l => l.Credit),
                    PostedAt = DateTime.UtcNow,
                    PostedBy = request.PostedBy,
                    FiscalPeriodId = activePeriod.Id,
                    Lines = lines
                };

                await _unitOfWork.JournalEntries.AddAsync(newEntry);
                await _unitOfWork.SaveChangesAsync();

                return Result<JournalEntry>.Ok(newEntry);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                // Race-condition safety net: a concurrent request may have beaten us.
                // Re-query and return the existing entry rather than surfacing a DB error.
                var racedEntries = await _unitOfWork.JournalEntries
                    .FindAsync(e =>
                        e.SourceType == request.SourceType &&
                        e.SourceId == request.SourceId);

                var racedEntry = racedEntries.FirstOrDefault();

                if (racedEntry != null)
                {
                    _logger.LogWarning(
                        "Duplicate posting caught by DB constraint (race): SourceType={SourceType}, SourceId={SourceId}",
                        request.SourceType,
                        request.SourceId);

                    return Result<JournalEntry>.Ok(racedEntry, AlreadyPostedMessage);
                }

                throw;
            }
        }

 

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            var innerMsg = ex.InnerException?.Message ?? string.Empty;

            return innerMsg.IndexOf("duplicate", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   innerMsg.IndexOf("unique", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   innerMsg.IndexOf("constraint", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private async Task<Result> ValidateJournalEntryAsync(
            List<JournalEntryLine> lines,
            DateTime date)
        {
            if (lines.Count < 2)
                return Result.Failure("Invalid journal entry: minimum 2 lines required.");

            // ── Zero-line guard ──────────────────────────────────────────────────────
            if (lines.Any(l => l.Debit == 0 && l.Credit == 0))
                return Result.Failure("Zero lines are not allowed.");

            // ── Double-entry balance ─────────────────────────────────────────────────
            var totalDebit  = Math.Round(lines.Sum(l => l.Debit),  6);
            var totalCredit = Math.Round(lines.Sum(l => l.Credit), 6);

            if (totalDebit != totalCredit)
                return Result.Failure(
                    $"Unbalanced entry: Debit ({totalDebit}) ≠ Credit ({totalCredit}). " +
                    $"Double-entry accounting violated.");

            // ── Leaf-account enforcement ─────────────────────────────────────────────
            var accountIds = lines.Select(l => l.AccountId).Distinct().ToList();
            var leafAccounts = await _unitOfWork.Accounts.GetLeafAccountsAsync();
            var leafIds = leafAccounts.Select(a => a.Id).ToHashSet();

            foreach (var accId in accountIds)
            {
                if (!leafIds.Contains(accId))
                    return Result.Failure(
                        $"Account {accId} is not a leaf account. " +
                        $"Posting to parent accounts is not allowed.");
            }

            // ── Fiscal period open check ─────────────────────────────────────────────
            var periodRes = await GetOpenFiscalPeriodAsync(date);

            if (!periodRes.Success)
                return Result.Failure(periodRes.Message);

            // ── Budget control ───────────────────────────────────────────────────────
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
                .FindAsync(p =>
                    !p.IsClosed &&
                    date >= p.StartDate &&
                    date <= p.EndDate);

            var activePeriod = openPeriods.FirstOrDefault();

            if (activePeriod == null)
                return Result<FiscalPeriod>.Failure(
                    "No open fiscal period found for the specified date.");

            return Result<FiscalPeriod>.Ok(activePeriod);
        }
    }
}
