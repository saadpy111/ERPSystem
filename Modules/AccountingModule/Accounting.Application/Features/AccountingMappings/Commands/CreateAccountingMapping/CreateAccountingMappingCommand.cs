using Accounting.Application.Features.AccountingMappings.DTOs;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.AccountingMappings.Commands.CreateAccountingMapping
{
    public class CreateAccountingMappingCommand : IRequest<Result<AccountingMappingDto>>
    {
        public SourceType SourceType { get; set; }
        public string MappingKey { get; set; } = null!;
        public int AccountId { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Description { get; set; }
    }

    public class CreateAccountingMappingCommandValidator : AbstractValidator<CreateAccountingMappingCommand>
    {
        public CreateAccountingMappingCommandValidator()
        {
            RuleFor(x => x.MappingKey).NotEmpty().MaximumLength(100);
            RuleFor(x => x.AccountId).GreaterThan(0);
        }
    }

    public class CreateAccountingMappingCommandHandler : IRequestHandler<CreateAccountingMappingCommand, Result<AccountingMappingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountingMappingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AccountingMappingDto>> Handle(CreateAccountingMappingCommand request, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            if (account == null)
                throw new System.Exception("Account does not exist.");

            var hasChildren = await _unitOfWork.Accounts.AnyAsync(a => a.ParentAccountId == request.AccountId);
            if (hasChildren)
                throw new System.Exception("Account must be a leaf account.");

            var duplicate = await _unitOfWork.AccountingMappings.AnyAsync(m =>
                m.SourceType == request.SourceType && m.MappingKey == request.MappingKey);
            if (duplicate)
                throw new System.Exception("Duplicate mapping key for this source type.");

            var entity = new AccountingMapping
            {
                SourceType = request.SourceType,
                MappingKey = request.MappingKey.Trim(),
                AccountId = request.AccountId,
                IsActive = request.IsActive,
                Description = request.Description
            };

            await _unitOfWork.AccountingMappings.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new AccountingMappingDto
            {
                Id = entity.Id,
                SourceType = entity.SourceType,
                MappingKey = entity.MappingKey,
                AccountId = entity.AccountId,
                AccountName = account.NameAr,
                IsActive = entity.IsActive,
                Description = entity.Description
            };
        }
    }
}
