using FluentValidation;
using Hr.Application.Features.PayrollRecordFeatures.UpdatePayrollRecord;

namespace Hr.Application.Validators
{
    public class UpdatePayrollRecordValidator : AbstractValidator<UpdatePayrollRecordRequest>
    {
        public UpdatePayrollRecordValidator()
        {
            RuleFor(x => x.PayrollId)
                .GreaterThan(0).WithMessage("Payroll ID is required");

            RuleFor(x => x.PeriodYear)
                .GreaterThan(0).WithMessage("Period year is required");

            RuleFor(x => x.PeriodMonth)
                .InclusiveBetween(1, 12).WithMessage("Period month must be between 1 and 12");
        }
    }
}