using FluentValidation;
using Hr.Application.Features.SalaryStructureFeatures.Commands.DeleteSalaryStructure;

namespace Hr.Application.Validators
{
    public class DeleteSalaryStructureValidator : AbstractValidator<DeleteSalaryStructureRequest>
    {
        public DeleteSalaryStructureValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Salary structure ID is required");
        }
    }
}