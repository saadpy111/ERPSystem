using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Fiscal.Queries.GetFiscalYears
{
    public class FiscalYearDto
    {
        public int Id { get; set; }
        public string YearName { get; set; } = null!;
    }

    public class GetFiscalYearsQuery : IRequest<Result<List<FiscalYearDto>>>
    {
    }

    public class GetFiscalYearsQueryHandler : IRequestHandler<GetFiscalYearsQuery, Result<List<FiscalYearDto>>>
    {
        private readonly IAccountingDbContext _context;

        public GetFiscalYearsQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<FiscalYearDto>>> Handle(GetFiscalYearsQuery request, CancellationToken cancellationToken)
        {
            return await _context.FiscalYears
                .AsNoTracking()
                .Select(f => new FiscalYearDto { Id = f.Id, YearName = f.Name })
                .ToListAsync(cancellationToken);
        }
    }
}
