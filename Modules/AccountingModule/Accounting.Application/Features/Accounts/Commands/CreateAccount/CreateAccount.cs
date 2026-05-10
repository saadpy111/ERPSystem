using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<Result<int>>
    {
        public string Code { get; set; } = null!;

        public string NameAr { get; set; } = null!;

        public string? NameEn { get; set; }

        public int? ParentAccountId { get; set; }

        public AccountType AccountType { get; set; }

        public int CurrencyId { get; set; }

        public bool IsGroup { get; set; }

        public bool AllowReconciliation { get; set; }
    }

    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty();

            RuleFor(x => x.NameAr)
                .NotEmpty();

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0);
        }
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
            var existingCode = await _context.Accounts
                .AnyAsync(a => a.Code == request.Code, cancellationToken);

            if (existingCode)
            {
                return Result<int>.Failure("Account code already exists.");
            }

            Account? parentAccount = null;

            if (request.ParentAccountId.HasValue)
            {
                parentAccount = await _context.Accounts
                    .FirstOrDefaultAsync(
                        a => a.Id == request.ParentAccountId.Value,
                        cancellationToken);

                if (parentAccount == null)
                {
                    return Result<int>.Failure("Parent account not found.");
                }

                if (!parentAccount.IsGroup)
                {
                    return Result<int>.Failure("Parent account must be a group account.");
                }

                if (parentAccount.AccountType != request.AccountType)
                {
                    return Result<int>.Failure("Child account type must match parent account type.");
                }
            }

            if (request.IsGroup && request.AllowReconciliation)
            {
                return Result<int>.Failure("Group accounts cannot allow reconciliation.");
            }

            var account = new Account
            {
                Code = request.Code,
                NameAr = request.NameAr,
                NameEn = request.NameEn,
                ParentAccountId = request.ParentAccountId,
                AccountType = request.AccountType,
                CurrencyId = request.CurrencyId,
                IsGroup = request.IsGroup,
                AllowReconciliation = request.AllowReconciliation,
                IsActive = true
            };

            await _context.Accounts.AddAsync(account, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<int>.Ok(account.Id, "Account created successfully");
        }
    }
}