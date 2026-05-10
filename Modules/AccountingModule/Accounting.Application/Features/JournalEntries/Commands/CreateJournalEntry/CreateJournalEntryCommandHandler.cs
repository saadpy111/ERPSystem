using Accounting.Application.Features.JournalEntries.DTOs;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using AutoMapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.JournalEntries.Commands.CreateJournalEntry
{
    public class CreateJournalEntryCommandHandler : IRequestHandler<CreateJournalEntryCommand, Result<JournalEntryResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<FiscalPeriod> _fiscalPeriodRepository;
        private readonly IMapper _mapper;

        public CreateJournalEntryCommandHandler(
            IUnitOfWork unitOfWork,
            IGenericRepository<FiscalPeriod> fiscalPeriodRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _fiscalPeriodRepository = fiscalPeriodRepository;
            _mapper = mapper;
        }

        public async Task<Result<JournalEntryResponseDto>> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
        {
            // Ensure accounts are leaf accounts
            var accountIds = request.Lines.Select(l => l.AccountId).Distinct().ToList();
            var allLeafs = await _unitOfWork.Accounts.GetLeafAccountsAsync();
            var allLeafIds = allLeafs.Select(a => a.Id).ToList();

            foreach (var accountId in accountIds)
            {
                if (!allLeafIds.Contains(accountId))
                {
                    return Result<JournalEntryResponseDto>.Failure($"Account with ID {accountId} is not a leaf account or does not exist.");
                }
            }

            // Ensure fiscal period is open
            var openPeriods = await _fiscalPeriodRepository.FindAsync(p => !p.IsClosed && request.Date >= p.StartDate && request.Date <= p.EndDate);
            var activePeriod = openPeriods.FirstOrDefault();
            
            if (activePeriod == null)
            {
                return Result<JournalEntryResponseDto>.Failure("No open fiscal period found for the specified date.");
            }

            var currency = await _unitOfWork.Currencies.GetByIdAsync(request.CurrencyId);
            if (currency == null || !currency.IsActive)
                return Result<JournalEntryResponseDto>.Failure("Invalid or inactive currency.");

            var journalEntry = _mapper.Map<JournalEntry>(request);

            journalEntry.FiscalPeriodId = activePeriod.Id;
            journalEntry.Status = JournalStatus.Draft;
            journalEntry.SourceType = SourceType.Manual;
            journalEntry.JournalNumber = "JE-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Raw amounts as provided by user - no conversion here per goal 
            foreach(var line in journalEntry.Lines)
            {
                line.CurrencyId = request.CurrencyId;
            }

            journalEntry.TotalDebit = journalEntry.Lines.Sum(l => l.Debit);
            journalEntry.TotalCredit = journalEntry.Lines.Sum(l => l.Credit);

            await _unitOfWork.JournalEntries.AddAsync(journalEntry);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<JournalEntryResponseDto>(journalEntry);
            return response;
        }
    }
}
