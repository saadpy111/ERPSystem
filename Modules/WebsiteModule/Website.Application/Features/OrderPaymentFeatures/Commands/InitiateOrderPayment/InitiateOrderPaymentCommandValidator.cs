using FluentValidation;

namespace Website.Application.Features.OrderPaymentFeatures.Commands.InitiateOrderPayment
{
    public class InitiateOrderPaymentCommandValidator : AbstractValidator<InitiateOrderPaymentCommand>
    {
        public InitiateOrderPaymentCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
