using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Accounting.Application.Features.Fiscal.Commands.CreateFiscalYear
{
    public class CreateFiscalYearCommand : IRequest<int>
    {
        public string YearName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateFiscalYearCommandValidator : AbstractValidator<CreateFiscalYearCommand>
    {
        public CreateFiscalYearCommandValidator()
        {
            RuleFor(x => x.YearName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate);
        }
    }

    public class CreateFiscalYearCommandHandler : IRequestHandler<CreateFiscalYearCommand, int>
    {
        private readonly IAccountingDbContext _context;

        public CreateFiscalYearCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateFiscalYearCommand request, CancellationToken cancellationToken)
        {
            var year = new FiscalYear
            {
                Name = request.YearName,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            await _context.FiscalYears.AddAsync(year, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return year.Id;
        }
    }
}
