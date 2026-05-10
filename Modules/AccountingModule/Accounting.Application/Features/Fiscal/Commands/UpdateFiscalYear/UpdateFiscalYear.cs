using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Fiscal.Commands.UpdateFiscalYear
{
    public class UpdateFiscalYearCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
    }

    public class UpdateFiscalYearCommandValidator : AbstractValidator<UpdateFiscalYearCommand>
    {
        public UpdateFiscalYearCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate)
                .WithMessage("StartDate must be earlier than EndDate.");
        }
    }

    public class UpdateFiscalYearCommandHandler : IRequestHandler<UpdateFiscalYearCommand, Result>
    {
        private readonly IAccountingDbContext _context;

        public UpdateFiscalYearCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateFiscalYearCommand request, CancellationToken cancellationToken)
        {
            var year = await _context.FiscalYears
                .FirstOrDefaultAsync(y => y.Id == request.Id && !y.IsDeleted, cancellationToken);

            if (year == null)
                return Result.Failure($"Fiscal year with ID {request.Id} was not found.");

            if (year.IsClosed)
                return Result.Failure("Cannot update a closed fiscal year.");

            year.Name = request.Name;
            year.StartDate = request.StartDate;
            year.EndDate = request.EndDate;
            year.UpdatedAt = System.DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
