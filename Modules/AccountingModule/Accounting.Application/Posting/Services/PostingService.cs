using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Posting.Interfaces;
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

        public PostingService(
            IEnumerable<IPostingStrategy> strategies,
            IUnitOfWork unitOfWork,
            IGenericRepository<FiscalPeriod> fiscalPeriodRepository)
        {
            _strategies = strategies;
            _unitOfWork = unitOfWork;
            _fiscalPeriodRepository = fiscalPeriodRepository;
        }

        public async Task<int> PostAsync(IPostingRequest request)
        {
            var strategy = _strategies.FirstOrDefault(s => s.CanHandle(request.SourceType));
            if (strategy == null)
            {
                throw new Exception($"No posting strategy found for SourceType {request.SourceType}");
            }

            var existingEntry = request.SourceType == SourceType.Manual
                ? await _unitOfWork.JournalEntries.GetByIdAsync(request.SourceId)
                : null;

            var lines = existingEntry != null 
                ? existingEntry.Lines.ToList() 
                : await strategy.GenerateLinesAsync(request);

            await ValidateJournalEntryAsync(lines, request.Date, request.CurrencyId);

            if (existingEntry != null)
            {
                existingEntry.Status = JournalStatus.Posted;
                existingEntry.PostedAt = DateTime.UtcNow;
                
                _unitOfWork.JournalEntries.Update(existingEntry);
                await _unitOfWork.SaveChangesAsync();
                
                return existingEntry.Id;
            }
            else
            {
                var activePeriod = await GetOpenFiscalPeriodAsync(request.Date);

                var newEntry = new JournalEntry
                {
                    JournalNumber = "JE-" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Date = request.Date,
                    Reference = request.Reference,
                    CurrencyId = request.CurrencyId,
                    SourceType = request.SourceType,
                    Description = request.Description,
                    Status = JournalStatus.Posted,
                    TotalDebit = lines.Sum(l => l.Debit),
                    TotalCredit = lines.Sum(l => l.Credit),
                    PostedAt = DateTime.UtcNow,
                    FiscalPeriodId = activePeriod.Id,
                    Lines = lines
                };

                await _unitOfWork.JournalEntries.AddAsync(newEntry);
                await _unitOfWork.SaveChangesAsync();
                
                return newEntry.Id;
            }
        }

        private async Task ValidateJournalEntryAsync(List<JournalEntryLine> lines, DateTime date, int currencyId)
        {
            if (lines == null || lines.Count < 2)
                throw new Exception("Journal entry must have at least two lines.");

            var totalDebit = lines.Sum(l => l.Debit);
            var totalCredit = lines.Sum(l => l.Credit);

            if (totalDebit != totalCredit)
                throw new Exception($"Total Debit ({totalDebit}) does not equal Total Credit ({totalCredit}).");

            if (lines.Any(l => l.Debit == 0 && l.Credit == 0))
                throw new Exception("No zero lines allowed.");

            // Check if accounts are leaf accounts
            var accountIds = lines.Select(l => l.AccountId).Distinct().ToList();
            var allLeafs = await _unitOfWork.Accounts.GetLeafAccountsAsync();
            var leafIds = allLeafs.Select(a => a.Id).ToList();

            foreach (var accId in accountIds)
            {
                if (!leafIds.Contains(accId))
                    throw new Exception($"Account {accId} is not a valid leaf account.");
            }

            // Verify currency
            var currency = await _unitOfWork.Currencies.GetByIdAsync(currencyId);
            if (currency == null || !currency.IsActive)
                throw new Exception("Invalid or inactive currency.");

            // Verify period
            await GetOpenFiscalPeriodAsync(date);
        }

        private async Task<FiscalPeriod> GetOpenFiscalPeriodAsync(DateTime date)
        {
            var openPeriods = await _fiscalPeriodRepository.FindAsync(p => !p.IsClosed && date >= p.StartDate && date <= p.EndDate);
            var activePeriod = openPeriods.FirstOrDefault();

            if (activePeriod == null)
            {
                throw new Exception("No open fiscal period found for the specified date.");
            }

            return activePeriod;
        }
    }
}
