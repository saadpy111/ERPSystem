using Accounting.Application.Features.AccountingMappings.DTOs;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Enums;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.AccountingMappings.Commands.UpdateAccountingMapping
{
    public class UpdateAccountingMappingCommand : IRequest<AccountingMappingDto>
    {
        public int Id { get; set; }
        public SourceType SourceType { get; set; }
        public string MappingKey { get; set; } = null!;
        public int AccountId { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateAccountingMappingCommandValidator : AbstractValidator<UpdateAccountingMappingCommand>
    {
        public UpdateAccountingMappingCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.MappingKey).NotEmpty().MaximumLength(100);
            RuleFor(x => x.AccountId).GreaterThan(0);
        }
    }

    public class UpdateAccountingMappingCommandHandler : IRequestHandler<UpdateAccountingMappingCommand, AccountingMappingDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAccountingMappingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountingMappingDto> Handle(UpdateAccountingMappingCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.AccountingMappings.GetByIdAsync(request.Id);
            if (entity == null)
                throw new System.Exception("Accounting mapping not found.");

            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
            if (account == null)
                throw new System.Exception("Account does not exist.");

            var hasChildren = await _unitOfWork.Accounts.AnyAsync(a => a.ParentAccountId == request.AccountId);
            if (hasChildren)
                throw new System.Exception("Account must be a leaf account.");

            var duplicate = await _unitOfWork.AccountingMappings.AnyAsync(m =>
                m.Id != request.Id &&
                m.SourceType == request.SourceType &&
                m.MappingKey == request.MappingKey);
            if (duplicate)
                throw new System.Exception("Duplicate mapping key for this source type.");

            entity.SourceType = request.SourceType;
            entity.MappingKey = request.MappingKey.Trim();
            entity.AccountId = request.AccountId;
            entity.IsActive = request.IsActive;
            entity.Description = request.Description;

            _unitOfWork.AccountingMappings.Update(entity);
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
