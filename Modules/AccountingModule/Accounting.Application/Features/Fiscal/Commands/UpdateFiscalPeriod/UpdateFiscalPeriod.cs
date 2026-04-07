using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Fiscal.Commands.UpdateFiscalPeriod
{
    public class UpdateFiscalPeriodCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string PeriodName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UpdateFiscalPeriodCommandValidator : AbstractValidator<UpdateFiscalPeriodCommand>
    {
        public UpdateFiscalPeriodCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.PeriodName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate)
                .WithMessage("StartDate must be earlier than EndDate.");
        }
    }

    public class UpdateFiscalPeriodCommandHandler : IRequestHandler<UpdateFiscalPeriodCommand, Unit>
    {
        private readonly IAccountingDbContext _context;

        public UpdateFiscalPeriodCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateFiscalPeriodCommand request, CancellationToken cancellationToken)
        {
            var period = await _context.FiscalPeriods
                .Include(p => p.FiscalYear)
                .FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, cancellationToken);

            if (period == null)
                throw new BusinessException($"Fiscal period with ID {request.Id} was not found.");

            if (period.IsClosed)
                throw new BusinessException("Cannot update a closed fiscal period.");

            if (period.FiscalYear.IsClosed)
                throw new BusinessException("Cannot update a period that belongs to a closed fiscal year.");

            // Validate dates stay within fiscal year range
            if (request.StartDate < period.FiscalYear.StartDate || request.EndDate > period.FiscalYear.EndDate)
                throw new BusinessException("Period dates must fall within the fiscal year range.");

            // Check for overlap with other periods (excluding self)
            bool overlaps = await _context.FiscalPeriods
                .AnyAsync(p => p.FiscalYearId == period.FiscalYearId
                            && p.Id != request.Id
                            && !p.IsDeleted
                            && p.StartDate < request.EndDate
                            && p.EndDate > request.StartDate,
                          cancellationToken);

            if (overlaps)
                throw new BusinessException("The updated period dates overlap with an existing period.");

            period.PeriodName = request.PeriodName;
            period.StartDate = request.StartDate;
            period.EndDate = request.EndDate;
            period.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
