using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Fiscal.Commands.CloseFiscalYear
{
    public class CloseFiscalYearCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class CloseFiscalYearCommandValidator : AbstractValidator<CloseFiscalYearCommand>
    {
        public CloseFiscalYearCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("A valid Fiscal Year ID must be provided.");
        }
    }

    public class CloseFiscalYearCommandHandler : IRequestHandler<CloseFiscalYearCommand, Unit>
    {
        private readonly IAccountingDbContext _context;

        public CloseFiscalYearCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CloseFiscalYearCommand request, CancellationToken cancellationToken)
        {
            var year = await _context.FiscalYears
                .Include(y => y.FiscalPeriods)
                .FirstOrDefaultAsync(y => y.Id == request.Id && !y.IsDeleted, cancellationToken);

            if (year == null)
                throw new BusinessException($"Fiscal year with ID {request.Id} was not found.");

            if (year.IsClosed)
                throw new BusinessException("Fiscal year is already closed.");

            // Business Rule: cannot close if any period is still open
            bool hasOpenPeriods = year.FiscalPeriods.Any(p => !p.IsClosed && !p.IsDeleted);
            if (hasOpenPeriods)
                throw new BusinessException("Cannot close the fiscal year while it has open periods. Please close all periods first.");

            year.IsClosed = true;
            year.UpdatedAt = System.DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
