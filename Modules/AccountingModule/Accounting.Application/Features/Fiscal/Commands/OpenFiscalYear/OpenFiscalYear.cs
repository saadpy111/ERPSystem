using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Fiscal.Commands.OpenFiscalYear
{
    public class OpenFiscalYearCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class OpenFiscalYearCommandValidator : AbstractValidator<OpenFiscalYearCommand>
    {
        public OpenFiscalYearCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("A valid Fiscal Year ID must be provided.");
        }
    }

    public class OpenFiscalYearCommandHandler : IRequestHandler<OpenFiscalYearCommand, Result>
    {
        private readonly IAccountingDbContext _context;

        public OpenFiscalYearCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(OpenFiscalYearCommand request, CancellationToken cancellationToken)
        {
            var year = await _context.FiscalYears
                .FirstOrDefaultAsync(y => y.Id == request.Id && !y.IsDeleted, cancellationToken);

            if (year == null)
                throw new BusinessException($"Fiscal year with ID {request.Id} was not found.");

            if (!year.IsClosed)
                throw new BusinessException("Fiscal year is already open.");

            year.IsClosed = false;
            year.UpdatedAt = System.DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
