using FluentValidation;

namespace Website.Application.Features.FavoriteFeatures.Commands.AddFavorite
{
    public class AddFavoriteCommandValidator : AbstractValidator<AddFavoriteCommand>
    {
        public AddFavoriteCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");
        }
    }
}
