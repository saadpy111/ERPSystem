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

namespace Accounting.Application.Features.JournalEntries.Commands.CreateJournalEntry
{
    public class CreateJournalEntryCommandHandler : IRequestHandler<CreateJournalEntryCommand, JournalEntryResponseDto>
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

        public async Task<JournalEntryResponseDto> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
        {
            // Ensure accounts are leaf accounts
            var accountIds = request.Lines.Select(l => l.AccountId).Distinct().ToList();
            var allLeafs = await _unitOfWork.Accounts.GetLeafAccountsAsync();
            var allLeafIds = allLeafs.Select(a => a.Id).ToList();

            foreach (var accountId in accountIds)
            {
                if (!allLeafIds.Contains(accountId))
                {
                    throw new Exception($"Account with ID {accountId} is not a leaf account or does not exist.");
                }
            }

            // Ensure fiscal period is open
            var openPeriods = await _fiscalPeriodRepository.FindAsync(p => !p.IsClosed && request.Date >= p.StartDate && request.Date <= p.EndDate);
            var activePeriod = openPeriods.FirstOrDefault();
            
            if (activePeriod == null)
            {
                throw new Exception("No open fiscal period found for the specified date.");
            }

            var totalDebit = request.Lines.Sum(l => l.Debit);
            var totalCredit = request.Lines.Sum(l => l.Credit);

            var journalEntry = _mapper.Map<JournalEntry>(request);

            journalEntry.FiscalPeriodId = activePeriod.Id;
            journalEntry.Status = JournalStatus.Draft; // Only Draft Journal Entries
            journalEntry.SourceType = SourceType.Manual;
            journalEntry.TotalDebit = totalDebit;
            journalEntry.TotalCredit = totalCredit;
            journalEntry.JournalNumber = "JE-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            await _unitOfWork.JournalEntries.AddAsync(journalEntry);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<JournalEntryResponseDto>(journalEntry);
            return response;
        }
    }
}
