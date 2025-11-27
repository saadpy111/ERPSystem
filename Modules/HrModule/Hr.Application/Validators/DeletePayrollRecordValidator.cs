using FluentValidation;
using Hr.Application.Features.PayrollRecordFeatures.DeletePayrollRecord;

namespace Hr.Application.Validators
{
    public class DeletePayrollRecordValidator : AbstractValidator<DeletePayrollRecordRequest>
    {
        public DeletePayrollRecordValidator()
        {
            RuleFor(x => x.PayrollId)
                .GreaterThan(0).WithMessage("Payroll ID is required");
        }
    }
}