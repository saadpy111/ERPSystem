using FluentValidation;
using Hr.Application.Features.EmployeeContractFeatures.Commands.UpdateEmployeeContract;

namespace Hr.Application.Validators
{
    public class UpdateEmployeeContractValidator : AbstractValidator<UpdateEmployeeContractRequest>
    {
        public UpdateEmployeeContractValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Contract ID is required");

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