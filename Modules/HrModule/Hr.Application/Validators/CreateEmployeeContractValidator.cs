using FluentValidation;
using Hr.Application.Features.EmployeeContractFeatures.Commands.CreateEmployeeContract;

namespace Hr.Application.Validators
{
    public class CreateEmployeeContractValidator : AbstractValidator<CreateEmployeeContractRequest>
    {
        public CreateEmployeeContractValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee is required");

            RuleFor(x => x.JobId)
                .GreaterThan(0).WithMessage("Job is required");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.Salary)
                .GreaterThan(0).WithMessage("Salary must be greater than 0");

            RuleFor(x => x.ContractType)
                .NotEmpty().WithMessage("Contract type is required");
        }
    }
}