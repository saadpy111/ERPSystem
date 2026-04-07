using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Fiscal.Commands.CreateFiscalPeriod
{
    public class CreateFiscalPeriodCommand : IRequest<int>
    {
        public int FiscalYearId { get; set; }
        public string PeriodName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateFiscalPeriodCommandValidator : AbstractValidator<CreateFiscalPeriodCommand>
    {
        public CreateFiscalPeriodCommandValidator()
        {
            RuleFor(x => x.FiscalYearId).GreaterThan(0).WithMessage("A valid Fiscal Year ID must be provided.");
            RuleFor(x => x.PeriodName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate)
                .WithMessage("StartDate must be earlier than EndDate.");
        }
    }

    public class CreateFiscalPeriodCommandHandler : IRequestHandler<CreateFiscalPeriodCommand, int>
    {
        private readonly IAccountingDbContext _context;

        public CreateFiscalPeriodCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateFiscalPeriodCommand request, CancellationToken cancellationToken)
        {
            // 1. Verify fiscal year exists and is open
            var year = await _context.FiscalYears
                .FirstOrDefaultAsync(y => y.Id == request.FiscalYearId && !y.IsDeleted, cancellationToken);

            if (year == null)
                throw new BusinessException($"Fiscal year with ID {request.FiscalYearId} was not found.");

            if (year.IsClosed)
                throw new BusinessException("Cannot add a period to a closed fiscal year.");

            // 2. Validate period falls within fiscal year range
            if (request.StartDate < year.StartDate || request.EndDate > year.EndDate)
                throw new BusinessException("Period dates must fall within the fiscal year range.");

            // 3. Check for overlapping periods
            bool overlaps = await _context.FiscalPeriods
                .AnyAsync(p => p.FiscalYearId == request.FiscalYearId
                            && !p.IsDeleted
                            && p.StartDate < request.EndDate
                            && p.EndDate > request.StartDate,
                          cancellationToken);

            if (overlaps)
                throw new BusinessException("The period dates overlap with an existing period in this fiscal year.");

            var period = new FiscalPeriod
            {
                FiscalYearId = request.FiscalYearId,
                PeriodName = request.PeriodName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsClosed = false
            };

            await _context.FiscalPeriods.AddAsync(period, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return period.Id;
        }
    }
}
