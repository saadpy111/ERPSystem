using FluentValidation;
using Hr.Application.Features.EmployeeFeatures.UpdateEmployee;

namespace Hr.Application.Validators
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeRequest>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Employee ID is required");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(150).WithMessage("Full name cannot exceed 150 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(50).WithMessage("Phone number cannot exceed 50 characters");
        }
    }
}