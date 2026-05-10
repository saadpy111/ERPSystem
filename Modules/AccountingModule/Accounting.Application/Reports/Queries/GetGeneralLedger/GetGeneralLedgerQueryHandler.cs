using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Reports.DTOs;
using Accounting.Domain.Enums;
using MediatR;
using Accounting.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Reports.Queries.GetGeneralLedger
{
    public class GetGeneralLedgerQueryHandler : IRequestHandler<GetGeneralLedgerQuery, Result<Accounting.Application.Reports.Models.ReportResponse<LedgerDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGeneralLedgerQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Accounting.Application.Reports.Models.ReportResponse<LedgerDto>>> Handle(GetGeneralLedgerQuery request, CancellationToken cancellationToken)
        {
            decimal openingBalance = 0m;

            // Fetch running balance mathematically for prior entries
            if (request.FromDate.HasValue)
            {
                var priorQuery = _unitOfWork.JournalEntryLines.Query()
                    .Where(l => l.AccountId == request.AccountId && 
                                l.JournalEntry.Status == JournalStatus.Posted && 
                                l.JournalEntry.Date < request.FromDate.Value);

                if (request.CostCenterId.HasValue)
                {
                    priorQuery = priorQuery.Where(l => l.CostCenterId == request.CostCenterId.Value);
                }

                var priorEntries = await priorQuery
                    .GroupBy(l => l.AccountId)
                    .Select(g => new { Debit = g.Sum(x => x.Debit), Credit = g.Sum(x => x.Credit) })
                    .FirstOrDefaultAsync(cancellationToken);

                if (priorEntries != null)
                {
                    // The generic opening balance is returned as absolute difference here; we'll refine it when we get the account type below
                    openingBalance = priorEntries.Debit - priorEntries.Credit;
                }
            }

            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            if (account == null)
            {
                return Result<Accounting.Application.Reports.Models.ReportResponse<LedgerDto>>.Failure("Account not found.");
            }
            
            if (account.AccountType == AccountType.Liability || account.AccountType == AccountType.Equity || account.AccountType == AccountType.Revenue)
            {
                openingBalance = -openingBalance; // For these accounts, credit is positive balance
            }

            var query = _unitOfWork.JournalEntryLines.Query()
                .Where(l => l.AccountId == request.AccountId && l.JournalEntry.Status == JournalStatus.Posted);

            if (request.CostCenterId.HasValue)
            {
                query = query.Where(l => l.CostCenterId == request.CostCenterId.Value);
            }

            if (request.FromDate.HasValue)
            {
                query = query.Where(l => l.JournalEntry.Date >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                query = query.Where(l => l.JournalEntry.Date <= request.ToDate.Value);
            }

            var entries = await query
                .Select(l => new
                {
                    Date = l.JournalEntry.Date,
                    JournalEntryId = l.JournalEntryId,
                    Reference = l.JournalEntry.Reference,
                    Description = l.Description ?? l.JournalEntry.Description ?? "N/A",
                    Debit = l.Debit,
                    Credit = l.Credit,
                    PartnerId = l.PartnerId,
                    CostCenterId = l.CostCenterId,
                    CostCenterName = l.CostCenter != null ? l.CostCenter.NameAr : null
                })
                .OrderBy(x => x.Date)
                .ThenBy(x => x.JournalEntryId)
                .ToListAsync(cancellationToken);

            var result = new List<LedgerDto>();
            
            if (request.FromDate.HasValue && openingBalance != 0m)
            {
                result.Add(new LedgerDto
                {
                    Date = request.FromDate.Value.AddDays(-1),
                    Reference = "OPENING",
                    Description = "Opening Balance",
                    Debit = openingBalance > 0m ? openingBalance : 0m,
                    Credit = openingBalance < 0m ? Math.Abs(openingBalance) : 0m,
                    RunningBalance = openingBalance,
                    PartnerId = null,
                    CostCenterId = request.CostCenterId
                });
            }

            decimal runningBalance = openingBalance;

            foreach (var entry in entries)
            {
                if (account.AccountType == AccountType.Liability || account.AccountType == AccountType.Equity || account.AccountType == AccountType.Revenue)
                {
                    runningBalance += (entry.Credit - entry.Debit);
                }
                else
                {
                    runningBalance += (entry.Debit - entry.Credit);
                }

                result.Add(new LedgerDto
                {
                    Date = entry.Date,
                    Reference = entry.Reference,
                    Description = entry.Description,
                    Debit = entry.Debit,
                    Credit = entry.Credit,
                    RunningBalance = runningBalance,
                    AccountType = account.AccountType,
                    PartnerId = entry.PartnerId,
                    CostCenterId = entry.CostCenterId,
                    CostCenterName = entry.CostCenterName
                });
            }

            var response = new Accounting.Application.Reports.Models.ReportResponse<LedgerDto>
            {
                Items = result,
                Metadata = new Accounting.Application.Reports.Models.ReportMetadata
                {
                    ReportName = $"General Ledger - {account.Code} - {account.NameEn ?? account.NameAr}",
                    FromDate = request.FromDate,
                    ToDate = request.ToDate
                }
            };
            
            response.Totals.Add("Total Debit", result.Sum(x => x.Debit));
            response.Totals.Add("Total Credit", result.Sum(x => x.Credit));
            response.Totals.Add("Ending Balance", runningBalance);

            return Result<Accounting.Application.Reports.Models.ReportResponse<LedgerDto>>.Ok(response);
        }
    }
}
