using FluentValidation;
using Hr.Application.Features.JobFeatures.ActivateJob;

namespace Hr.Application.Validators
{
    public class ActivateJobValidator : AbstractValidator<ActivateJobRequest>
    {
        public ActivateJobValidator()
        {
            RuleFor(x => x.JobId)
                .GreaterThan(0).WithMessage("Job ID is required");
        }
    }
}