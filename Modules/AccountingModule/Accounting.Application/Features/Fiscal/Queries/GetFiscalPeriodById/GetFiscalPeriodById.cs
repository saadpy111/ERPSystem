using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Fiscal.Queries.GetFiscalPeriodById
{
    public class FiscalPeriodDetailDto
    {
        public int Id { get; set; }
        public int FiscalYearId { get; set; }
        public string PeriodName { get; set; } = null!;
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
    }

    public class GetFiscalPeriodByIdQuery : IRequest<FiscalPeriodDetailDto>
    {
        public int Id { get; set; }
    }

    public class GetFiscalPeriodByIdQueryHandler : IRequestHandler<GetFiscalPeriodByIdQuery, FiscalPeriodDetailDto>
    {
        private readonly IAccountingDbContext _context;

        public GetFiscalPeriodByIdQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<FiscalPeriodDetailDto> Handle(GetFiscalPeriodByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = await _context.FiscalPeriods
                .AsNoTracking()
                .Where(p => p.Id == request.Id && !p.IsDeleted)
                .Select(p => new FiscalPeriodDetailDto
                {
                    Id = p.Id,
                    FiscalYearId = p.FiscalYearId,
                    PeriodName = p.PeriodName,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    IsClosed = p.IsClosed
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
                throw new BusinessException($"Fiscal period with ID {request.Id} was not found.");

            return dto;
        }
    }
}
