using FluentValidation;
using Hr.Application.Features.PayrollRecordFeatures.CreatePayrollRecord;

namespace Hr.Application.Validators
{
    public class CreatePayrollRecordValidator : AbstractValidator<CreatePayrollRecordRequest>
    {
        public CreatePayrollRecordValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee is required");

            RuleFor(x => x.PeriodYear)
                .GreaterThan(0).WithMessage("Period year is required");

            RuleFor(x => x.PeriodMonth)
                .InclusiveBetween(1, 12).WithMessage("Period month must be between 1 and 12");

            RuleFor(x => x.BaseSalary)
                .GreaterThanOrEqualTo(0).WithMessage("Base salary cannot be negative");

            RuleFor(x => x.TotalAllowances)
                .GreaterThanOrEqualTo(0).WithMessage("Total allowances cannot be negative");

            RuleFor(x => x.TotalDeductions)
                .GreaterThanOrEqualTo(0).WithMessage("Total deductions cannot be negative");
        }
    }
}
