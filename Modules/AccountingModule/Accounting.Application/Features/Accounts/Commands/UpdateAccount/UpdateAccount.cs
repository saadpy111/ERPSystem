using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Common.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
    }

    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(100);
        }
    }

    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Unit>
    {
        private readonly IAccountingDbContext _context;

        public UpdateAccountCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
            if (account == null) throw new BusinessException("Not found");

            account.NameAr = request.NameAr;
            account.NameEn = request.NameEn;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
