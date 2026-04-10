using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.CostCenters.Commands.UpdateCostCenter
{
    public class UpdateCostCenterCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateCostCenterCommandValidator : AbstractValidator<UpdateCostCenterCommand>
    {
        public UpdateCostCenterCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);

            RuleFor(x => x)
                .MustAsync(async (cmd, ct) => await unitOfWork.CostCenters.IsCodeUniqueAsync(cmd.Code, cmd.Id))
                .WithMessage("Cost Center code must be unique.");

            RuleFor(x => x)
                .Must(cmd => cmd.ParentId != cmd.Id)
                .WithMessage("Cost Center cannot be its own parent.");

            RuleFor(x => x)
                .MustAsync(async (cmd, ct) => 
                {
                    if (!cmd.ParentId.HasValue) return true;
                    var parent = await unitOfWork.CostCenters.GetByIdAsync(cmd.ParentId.Value);
                    if (parent == null || !parent.IsActive) return false;
                    
                    return !await unitOfWork.CostCenters.IsDescendantAsync(cmd.Id, cmd.ParentId.Value);
                })
                .WithMessage("Parent Cost Center must exist, be active, and not be a descendant.");
        }
    }

    public class UpdateCostCenterCommandHandler : IRequestHandler<UpdateCostCenterCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCostCenterCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(UpdateCostCenterCommand request, CancellationToken cancellationToken)
        {
            var cc = await _unitOfWork.CostCenters.GetByIdAsync(request.Id);
            if (cc == null) return Result<int>.Failure("Cost Center not found.");

            int? level = 1;
            if (request.ParentId.HasValue)
            {
                var parent = await _unitOfWork.CostCenters.GetByIdAsync(request.ParentId.Value);
                level = (parent?.Level ?? 0) + 1;
            }

            cc.Code = request.Code;
            cc.NameAr = request.NameAr;
            cc.NameEn = request.NameEn;
            cc.ParentId = request.ParentId;
            cc.Level = level;
            cc.IsActive = request.IsActive;

            _unitOfWork.CostCenters.Update(cc);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Ok(cc.Id, "Cost Center updated successfully.");
        }
    }
}
