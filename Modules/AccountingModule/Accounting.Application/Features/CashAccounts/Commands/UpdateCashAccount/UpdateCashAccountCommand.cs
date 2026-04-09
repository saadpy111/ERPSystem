using Accounting.Application.Features.CashAccounts.DTOs;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Enums;
using Accounting.Domain.Enums;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.CashAccounts.Commands.UpdateCashAccount
{
    public class UpdateCashAccountCommand : IRequest<Result<CashAccountDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int AccountId { get; set; }
    }

    public class UpdateCashAccountCommandHandler : IRequestHandler<UpdateCashAccountCommand, Result<CashAccountDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCashAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CashAccountDto>> Handle(UpdateCashAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.CashAccounts.GetByIdAsync(request.Id);
            if (entity == null) return Result<CashAccountDto>.Failure("Cash account not found.");

            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            
            entity.Name = request.Name.Trim();
            entity.AccountId = request.AccountId;

            _unitOfWork.CashAccounts.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<CashAccountDto>.IsSuccess(new CashAccountDto
            {
                Id = entity.Id,
                Name = entity.Name,
                AccountId = entity.AccountId,
                AccountCode = account.Code,
                AccountName = account.NameAr
            }, "Cash account updated successfully.");
        }
    }
}
