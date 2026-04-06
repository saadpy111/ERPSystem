using Accounting.Application.Features.CashAccounts.DTOs;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.CashAccounts.Commands.CreateCashAccount
{
    public class CreateCashAccountCommand : IRequest<CashAccountDto>
    {
        public string Name { get; set; } = null!;
        public int AccountId { get; set; }
    }

    public class CreateCashAccountCommandValidator : AbstractValidator<CreateCashAccountCommand>
    {
        public CreateCashAccountCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.AccountId).GreaterThan(0);
        }
    }

    public class CreateCashAccountCommandHandler : IRequestHandler<CreateCashAccountCommand, CashAccountDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCashAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CashAccountDto> Handle(CreateCashAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            if (account == null)
                throw new System.Exception("Account does not exist.");

            if (account.IsGroup)
                throw new System.Exception("Account must be leaf (not group).");

            if (account.AccountType != AccountType.Asset)
                throw new System.Exception("Cash account must map to an Asset account.");

            var entity = new CashAccount
            {
                Name = request.Name.Trim(),
                AccountId = request.AccountId
            };

            await _unitOfWork.CashAccounts.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new CashAccountDto
            {
                Id = entity.Id,
                Name = entity.Name,
                AccountId = entity.AccountId,
                AccountCode = account.Code,
                AccountName = account.NameAr
            };
        }
    }
}
