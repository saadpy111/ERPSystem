using Accounting.Application.Features.CashAccounts.DTOs;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Enums;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.CashAccounts.Commands.UpdateCashAccount
{
    public class UpdateCashAccountCommand : IRequest<CashAccountDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int AccountId { get; set; }
    }

    public class UpdateCashAccountCommandValidator : AbstractValidator<UpdateCashAccountCommand>
    {
        public UpdateCashAccountCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.AccountId).GreaterThan(0);
        }
    }

    public class UpdateCashAccountCommandHandler : IRequestHandler<UpdateCashAccountCommand, CashAccountDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCashAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CashAccountDto> Handle(UpdateCashAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.CashAccounts.GetByIdAsync(request.Id);
            if (entity == null)
                throw new System.Exception("Cash account not found.");

            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            if (account == null)
                throw new System.Exception("Account does not exist.");

            if (account.IsGroup)
                throw new System.Exception("Account must be leaf (not group).");

            if (account.AccountType != AccountType.Asset)
                throw new System.Exception("Cash account must map to an Asset account.");

            entity.Name = request.Name.Trim();
            entity.AccountId = request.AccountId;

            _unitOfWork.CashAccounts.Update(entity);
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
