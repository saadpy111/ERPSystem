using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Entities;
using FluentValidation;
using MediatR;

using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;

namespace Accounting.Application.Features.CostCenters.Commands.CreateCostCenter
{
    public class CreateCostCenterCommand : IRequest<Result<int>>
    {
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public int? ParentId { get; set; }
    }

    public class CreateCostCenterCommandValidator : AbstractValidator<CreateCostCenterCommand>
    {
        public CreateCostCenterCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NameEn).MaximumLength(200);

            RuleFor(x => x.Code)
                .MustAsync(async (code, ct) => await unitOfWork.CostCenters.IsCodeUniqueAsync(code))
                .WithMessage("Cost Center code must be unique.");

            RuleFor(x => x.ParentId)
                .MustAsync(async (parentId, ct) => 
                {
                    if (!parentId.HasValue) return true;
                    var parent = await unitOfWork.CostCenters.GetByIdAsync(parentId.Value);
                    return parent != null && parent.IsActive;
                })
                .WithMessage("Parent Cost Center must exist and be active.");
        }
    }

    public class CreateCostCenterCommandHandler : IRequestHandler<CreateCostCenterCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCostCenterCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateCostCenterCommand request, CancellationToken cancellationToken)
        {
            int? level = 1;
            if (request.ParentId.HasValue)
            {
                var parent = await _unitOfWork.CostCenters.GetByIdAsync(request.ParentId.Value);
                level = (parent?.Level ?? 0) + 1;
            }

            var cc = new CostCenter
            {
                Code = request.Code,
                NameAr = request.NameAr,
                NameEn = request.NameEn,
                ParentId = request.ParentId,
                Level = level,
                IsActive = true
            };

            await _unitOfWork.CostCenters.AddAsync(cc);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Ok(cc.Id, "Cost Center created successfully.");
        }
    }
}
