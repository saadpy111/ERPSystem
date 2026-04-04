using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Fiscal.Queries.GetFiscalPeriods
{
    public class FiscalPeriodDto
    {
        public int Id { get; set; }
        public string PeriodName { get; set; } = null!;
        public bool IsClosed { get; set; }
    }

    public class GetFiscalPeriodsQuery : IRequest<List<FiscalPeriodDto>>
    {
        public int? FiscalYearId { get; set; }
    }

    public class GetFiscalPeriodsQueryHandler : IRequestHandler<GetFiscalPeriodsQuery, List<FiscalPeriodDto>>
    {
        private readonly IAccountingDbContext _context;

        public GetFiscalPeriodsQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<List<FiscalPeriodDto>> Handle(GetFiscalPeriodsQuery request, CancellationToken cancellationToken)
        {
            var q = _context.FiscalPeriods.AsNoTracking();

            if (request.FiscalYearId.HasValue)
            {
                q = q.Where(p => p.FiscalYearId == request.FiscalYearId.Value);
            }

            return await q
                .Select(p => new FiscalPeriodDto { Id = p.Id, PeriodName = p.PeriodName, IsClosed = p.IsClosed })
                .ToListAsync(cancellationToken);
        }
    }
}
