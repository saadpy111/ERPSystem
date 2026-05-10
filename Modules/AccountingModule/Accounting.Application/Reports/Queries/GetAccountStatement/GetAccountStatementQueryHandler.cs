using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
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

namespace Accounting.Application.Reports.Queries.GetAccountStatement
{
    public class GetAccountStatementQueryHandler : IRequestHandler<GetAccountStatementQuery, Result<Accounting.Application.Reports.Models.ReportResponse<AccountStatementDto>>>
    {
        private readonly IAccountingDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public GetAccountStatementQueryHandler(IAccountingDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Accounting.Application.Reports.Models.ReportResponse<AccountStatementDto>>> Handle(GetAccountStatementQuery request, CancellationToken cancellationToken)
        {
            var partner = await _unitOfWork.Partners.GetByIdAsync(request.PartnerId);
            if (partner == null)
            {
                return Result<Accounting.Application.Reports.Models.ReportResponse<AccountStatementDto>>.Failure($"Partner with ID {request.PartnerId} not found.");
            }

            decimal openingBalance = 0m;

            if (request.FromDate.HasValue)
            {
                var priorEntries = await _context.JournalEntryLines
                    .AsNoTracking()
                    .Where(l => l.PartnerId == request.PartnerId && 
                                l.JournalEntry.Status == JournalStatus.Posted && 
                                l.JournalEntry.Date < request.FromDate.Value)
                    .GroupBy(l => l.PartnerId)
                    .Select(g => new { 
                        TotalDebit = g.Sum(x => x.Debit),
                        TotalCredit = g.Sum(x => x.Credit) 
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (priorEntries != null)
                {
                    if (partner.Type == PartnerType.Customer)
                    {
                        openingBalance = priorEntries.TotalDebit - priorEntries.TotalCredit;
                    }
                    else // Vendor
                    {
                        openingBalance = priorEntries.TotalCredit - priorEntries.TotalDebit;
                    }
                }
            }

            var query = _context.JournalEntryLines
                .AsNoTracking()
                .Where(l => l.PartnerId == request.PartnerId && l.JournalEntry.Status == JournalStatus.Posted);

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
                    SourceType = l.JournalEntry.SourceType
                })
                .OrderBy(x => x.Date)
                .ThenBy(x => x.JournalEntryId)
                .ToListAsync(cancellationToken);

            var result = new List<AccountStatementDto>();
            
            if (request.FromDate.HasValue && openingBalance != 0m)
            {
                result.Add(new AccountStatementDto
                {
                    Date = request.FromDate.Value.AddDays(-1),
                    Reference = "OPENING",
                    Description = "Opening Balance",
                    Debit = partner.Type == PartnerType.Customer 
                        ? (openingBalance > 0m ? openingBalance : 0m)
                        : (openingBalance < 0m ? Math.Abs(openingBalance) : 0m),
                    Credit = partner.Type == PartnerType.Customer
                        ? (openingBalance < 0m ? Math.Abs(openingBalance) : 0m)
                        : (openingBalance > 0m ? openingBalance : 0m),
                    RunningBalance = openingBalance,
                    SourceType = null
                });
            }

            decimal runningBalance = openingBalance;

            foreach (var entry in entries)
            {
                if (partner.Type == PartnerType.Customer)
                {
                    runningBalance += (entry.Debit - entry.Credit);
                }
                else // Vendor
                {
                    runningBalance += (entry.Credit - entry.Debit);
                }

                result.Add(new AccountStatementDto
                {
                    Date = entry.Date,
                    Reference = entry.Reference,
                    Description = entry.Description,
                    Debit = entry.Debit,
                    Credit = entry.Credit,
                    RunningBalance = runningBalance,
                    SourceType = entry.SourceType
                });
            }

            var response = new Accounting.Application.Reports.Models.ReportResponse<AccountStatementDto>
            {
                Items = result,
                Metadata = new Accounting.Application.Reports.Models.ReportMetadata
                {
                    ReportName = $"Account Statement - {partner.NameEn ?? partner.NameAr}",
                    FromDate = request.FromDate,
                    ToDate = request.ToDate
                }
            };
            
            response.Totals.Add("Total Debit", result.Sum(x => x.Debit));
            response.Totals.Add("Total Credit", result.Sum(x => x.Credit));
            response.Totals.Add("Ending Balance", runningBalance);

            return Result<Accounting.Application.Reports.Models.ReportResponse<AccountStatementDto>>.Ok(response);
        }
    }
}
