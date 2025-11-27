using FluentValidation;
using Hr.Application.Features.JobFeatures.DeactivateJob;

namespace Hr.Application.Validators
{
    public class DeactivateJobValidator : AbstractValidator<DeactivateJobRequest>
    {
        public DeactivateJobValidator()
        {
            RuleFor(x => x.JobId)
                .GreaterThan(0).WithMessage("Job ID is required");
        }
    }
}