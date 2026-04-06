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

        public PostingService(
            IEnumerable<IPostingStrategy> strategies,
            IUnitOfWork unitOfWork,
            IGenericRepository<FiscalPeriod> fiscalPeriodRepository,
            IExchangeRateService exchangeRateService)
        {
            _strategies = strategies;
            _unitOfWork = unitOfWork;
            _fiscalPeriodRepository = fiscalPeriodRepository;
            _exchangeRateService = exchangeRateService;
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

            var currency = await _unitOfWork.Currencies.GetByIdAsync(request.CurrencyId);
            if (currency == null || !currency.IsActive)
                throw new Exception("Invalid or inactive currency.");

            // Resolve Exchange Rate and Convert lines to Base Currency before validation/save
            var rate = await _exchangeRateService.GetRateAsync(request.CurrencyId, request.Date);

            foreach (var line in lines)
            {
                var foreignAmount = line.Debit != 0 ? line.Debit : line.Credit;
                
                line.CurrencyId = request.CurrencyId;
                line.ExchangeRate = rate;
                line.ForeignAmount = foreignAmount;
                line.BaseAmount = foreignAmount * rate;

                if (line.Debit != 0) line.Debit = line.BaseAmount;
                if (line.Credit != 0) line.Credit = line.BaseAmount;
            }

            await ValidateJournalEntryAsync(lines, request.Date, request.CurrencyId);

            if (existingEntry != null)
            {
                existingEntry.Status = JournalStatus.Posted;
                existingEntry.PostedAt = DateTime.UtcNow;
                existingEntry.CurrencyId = request.CurrencyId;
                existingEntry.TotalDebit = lines.Sum(l => l.Debit);
                existingEntry.TotalCredit = lines.Sum(l => l.Credit);
                
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

            var totalDebit = Math.Round(lines.Sum(l => l.Debit), 6);
            var totalCredit = Math.Round(lines.Sum(l => l.Credit), 6);

            if (totalDebit != totalCredit)
                throw new Exception($"Total Debit ({totalDebit}) does not equal Total Credit ({totalCredit}) in Base Currency.");

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
