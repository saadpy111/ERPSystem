using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Fiscal.Commands.DeleteFiscalPeriod
{
    public class DeleteFiscalPeriodCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteFiscalPeriodCommandValidator : AbstractValidator<DeleteFiscalPeriodCommand>
    {
        public DeleteFiscalPeriodCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("A valid Fiscal Period ID must be provided.");
        }
    }

    public class DeleteFiscalPeriodCommandHandler : IRequestHandler<DeleteFiscalPeriodCommand, Result>
    {
        private readonly IAccountingDbContext _context;

        public DeleteFiscalPeriodCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteFiscalPeriodCommand request, CancellationToken cancellationToken)
        {
            var period = await _context.FiscalPeriods
                .FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, cancellationToken);

            if (period == null)
                return Result.Failure($"Fiscal period with ID {request.Id} was not found.");

            if (period.IsClosed)
                return Result.Failure("Cannot delete a closed fiscal period.");

            // Soft delete
            period.IsDeleted = true;
            period.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
