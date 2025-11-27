using FluentValidation;
using Hr.Application.Features.PayrollComponentFeatures.DeletePayrollComponent;

namespace Hr.Application.Validators
{
    public class DeletePayrollComponentValidator : AbstractValidator<DeletePayrollComponentRequest>
    {
        public DeletePayrollComponentValidator()
        {
            RuleFor(x => x.ComponentId)
                .GreaterThan(0).WithMessage("Component ID is required");
        }
    }
}