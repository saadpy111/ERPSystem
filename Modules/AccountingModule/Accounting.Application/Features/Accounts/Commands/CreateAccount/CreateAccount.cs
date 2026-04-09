using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Entities;
using FluentValidation;
using Accounting.Domain.Enums;
using MediatR;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<Result<int>>
    {
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public AccountType AccountType { get; set; }
        public int CurrencyId { get; set; }
        public bool IsGroup { get; set; }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<int>>
    {
        private readonly IAccountingDbContext _context;

        public CreateAccountCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                Code = request.Code,
                NameAr = request.NameAr,
                NameEn = request.NameEn,
                AccountType = request.AccountType,
                CurrencyId = request.CurrencyId,
                IsGroup = request.IsGroup,
                IsActive = true
            };

            await _context.Accounts.AddAsync(account, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.IsSuccess(account.Id, "Account created successfully");
        }
    }
}
