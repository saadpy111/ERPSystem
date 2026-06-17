using FluentValidation;

namespace Website.Application.Features.NewsletterFeatures.Commands.SubscribeNewsletter
{
    public class SubscribeNewsletterCommandValidator : AbstractValidator<SubscribeNewsletterCommand>
    {
        public SubscribeNewsletterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(256).WithMessage("Email must not exceed 256 characters.")
                .EmailAddress().WithMessage("A valid email address is required.");
        }
    }
}
