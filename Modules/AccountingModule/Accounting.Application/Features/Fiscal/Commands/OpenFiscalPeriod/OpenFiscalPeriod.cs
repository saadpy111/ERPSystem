using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Fiscal.Commands.OpenFiscalPeriod
{
    public class OpenFiscalPeriodCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class OpenFiscalPeriodCommandHandler : IRequestHandler<OpenFiscalPeriodCommand, Result>
    {
        private readonly IAccountingDbContext _context;

        public OpenFiscalPeriodCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(OpenFiscalPeriodCommand request, CancellationToken cancellationToken)
        {
            var period = await _context.FiscalPeriods.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (period == null) return Result.Failure("Fiscal period not found.");

            period.IsClosed = false;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
