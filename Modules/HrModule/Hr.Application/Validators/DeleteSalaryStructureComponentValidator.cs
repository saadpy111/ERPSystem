using FluentValidation;
using Hr.Application.Features.SalaryStructureComponentFeatures.Commands.DeleteSalaryStructureComponent;

namespace Hr.Application.Validators
{
    public class DeleteSalaryStructureComponentValidator : AbstractValidator<DeleteSalaryStructureComponentRequest>
    {
        public DeleteSalaryStructureComponentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Component ID is required");
        }
    }
}