using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Fiscal.Queries.GetFiscalYearById
{
    public class FiscalYearDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
    }

    public class GetFiscalYearByIdQuery : IRequest<Result<FiscalYearDetailDto>>
    {
        public int Id { get; set; }
    }

    public class GetFiscalYearByIdQueryHandler : IRequestHandler<GetFiscalYearByIdQuery, Result<FiscalYearDetailDto>>
    {
        private readonly IAccountingDbContext _context;

        public GetFiscalYearByIdQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<FiscalYearDetailDto>> Handle(GetFiscalYearByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = await _context.FiscalYears
                .AsNoTracking()
                .Where(y => y.Id == request.Id && !y.IsDeleted)
                .Select(y => new FiscalYearDetailDto
                {
                    Id = y.Id,
                    Name = y.Name,
                    StartDate = y.StartDate,
                    EndDate = y.EndDate,
                    IsClosed = y.IsClosed
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
                return Result<FiscalYearDetailDto>.Failure($"Fiscal year with ID {request.Id} was not found.");

            return dto;
        }
    }
}
