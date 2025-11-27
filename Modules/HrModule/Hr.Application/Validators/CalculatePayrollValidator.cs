using FluentValidation;
using Hr.Application.Features.PayrollRecordFeatures.Commands.CalculatePayroll;

namespace Hr.Application.Validators
{
    public class CalculatePayrollValidator : AbstractValidator<CalculatePayrollRequest>
    {
        public CalculatePayrollValidator()
        {
            RuleFor(x => x.PayrollRecordId)
                .GreaterThan(0).WithMessage("Payroll record ID is required");
        }
    }
}