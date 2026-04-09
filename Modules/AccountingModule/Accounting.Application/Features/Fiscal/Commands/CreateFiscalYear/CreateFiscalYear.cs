using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Fiscal.Commands.CreateFiscalYear
{
    public class CreateFiscalYearCommand : IRequest<Result<int>>
    {
        public string YearName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateFiscalYearCommandValidator : AbstractValidator<CreateFiscalYearCommand>
    {
        public CreateFiscalYearCommandValidator()
        {
            RuleFor(x => x.YearName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate)
                .WithMessage("StartDate must be earlier than EndDate.");
        }
    }

    public class CreateFiscalYearCommandHandler : IRequestHandler<CreateFiscalYearCommand, Result<int>>
    {
        private readonly IAccountingDbContext _context;

        public CreateFiscalYearCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(CreateFiscalYearCommand request, CancellationToken cancellationToken)
        {
            // Business Rule: no overlapping fiscal years per tenant
            bool overlaps = await _context.FiscalYears
                .AnyAsync(y => !y.IsDeleted
                            && y.StartDate < request.EndDate
                            && y.EndDate > request.StartDate,
                          cancellationToken);

            if (overlaps)
                throw new BusinessException("The fiscal year dates overlap with an existing fiscal year.");

            var year = new FiscalYear
            {
                Name = request.YearName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsClosed = false
            };

            // Auto-generate 12 monthly periods
            var periods = GenerateMonthlyPeriods(request.StartDate, request.EndDate);
            foreach (var period in periods)
                year.FiscalPeriods.Add(period);

            await _context.FiscalYears.AddAsync(year, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return year.Id;
        }

        /// <summary>
        /// Generates monthly fiscal periods aligned to the fiscal year's start date.
        /// Produces one period per calendar month covered by the fiscal year range,
        /// capped at 12 periods.
        /// </summary>
        private static List<FiscalPeriod> GenerateMonthlyPeriods(DateTime startDate, DateTime endDate)
        {
            var periods = new List<FiscalPeriod>();
            var monthStart = new DateTime(startDate.Year, startDate.Month, 1);
            int count = 0;

            while (monthStart < endDate && count < 12)
            {
                // Last day of this calendar month
                var monthEnd = new DateTime(monthStart.Year, monthStart.Month,
                    DateTime.DaysInMonth(monthStart.Year, monthStart.Month));

                // Clamp to fiscal year boundaries
                var periodStart = monthStart < startDate ? startDate : monthStart;
                var periodEnd   = monthEnd   > endDate   ? endDate   : monthEnd;

                periods.Add(new FiscalPeriod
                {
                    PeriodName = monthStart.ToString("MMMM yyyy"),
                    StartDate  = periodStart,
                    EndDate    = periodEnd,
                    IsClosed   = false
                });

                monthStart = monthStart.AddMonths(1);
                count++;
            }

            return periods;
        }
    }
}

