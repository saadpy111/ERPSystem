using FluentValidation;
using Hr.Application.Features.JobFeatures.DeleteJob;

namespace Hr.Application.Validators
{
    public class DeleteJobValidator : AbstractValidator<DeleteJobRequest>
    {
        public DeleteJobValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Job ID is required");
        }
    }
}