using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Entities;
using FluentValidation;
using Accounting.Domain.Enums;
using MediatR;

namespace Accounting.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<int>
    {
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public AccountType AccountType { get; set; }
        public int CurrencyId { get; set; }
        public bool IsGroup { get; set; }
        public int TenantId { get; set; }
    }

    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CurrencyId).GreaterThan(0);
        }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
    {
        private readonly IAccountingDbContext _context;

        public CreateAccountCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                Code = request.Code,
                NameAr = request.NameAr,
                NameEn = request.NameEn,
                AccountType = request.AccountType,
                CurrencyId = request.CurrencyId,
                IsGroup = request.IsGroup,
                TenantId = request.TenantId,
                IsActive = true
            };

            await _context.Accounts.AddAsync(account, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return account.Id;
        }
    }
}
