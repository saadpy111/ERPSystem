using FluentValidation;
using Hr.Application.Features.EmployeeContractFeatures.Commands.DeleteEmployeeContract;

namespace Hr.Application.Validators
{
    public class DeleteEmployeeContractValidator : AbstractValidator<DeleteEmployeeContractRequest>
    {
        public DeleteEmployeeContractValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Contract ID is required");
        }
    }
}