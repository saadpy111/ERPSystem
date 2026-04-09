using Accounting.Application.Features.CashAccounts.DTOs;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using FluentValidation;
using Accounting.Domain.Enums;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.CashAccounts.Commands.CreateCashAccount
{
    public class CreateCashAccountCommand : IRequest<Result<CashAccountDto>>
    {
        public string Name { get; set; } = null!;
        public int AccountId { get; set; }
    }

    public class CreateCashAccountCommandHandler : IRequestHandler<CreateCashAccountCommand, Result<CashAccountDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCashAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CashAccountDto>> Handle(CreateCashAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            
            var entity = new CashAccount
            {
                Name = request.Name.Trim(),
                AccountId = request.AccountId
            };

            await _unitOfWork.CashAccounts.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<CashAccountDto>.IsSuccess(new CashAccountDto
            {
                Id = entity.Id,
                Name = entity.Name,
                AccountId = entity.AccountId,
                AccountCode = account.Code,
                AccountName = account.NameAr
            }, "Cash account created successfully.");
        }
    }
}
