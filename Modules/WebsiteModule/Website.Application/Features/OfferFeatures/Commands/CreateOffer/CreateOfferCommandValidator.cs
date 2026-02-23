using FluentValidation;
using Website.Domain.Enums;

namespace Website.Application.Features.OfferFeatures.Commands.CreateOffer
{
    public class CreateOfferCommandValidator : AbstractValidator<CreateOfferCommandRequest>
    {
        public CreateOfferCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("Discount value must be greater than 0.");

            RuleFor(x => x.DiscountType)
                .IsInEnum().WithMessage("Invalid discount type.");

            RuleFor(x => x.DiscountValue)
                .InclusiveBetween(0.01m, 100m)
                .When(x => x.DiscountType == DiscountType.Percentage)
                .WithMessage("Percentage discount must be between 0.01 and 100.");

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate).WithMessage("Start date must be before end date.");

            RuleFor(x => x.Priority)
                .GreaterThanOrEqualTo(0).WithMessage("Priority must be 0 or greater.");

            RuleFor(x => x.ScopeType)
                .IsInEnum().WithMessage("Invalid scope type.");

            // Scope specific rules
            RuleFor(x => x.ProductIds)
                .Empty()
                .When(x => x.ScopeType == OfferScopeType.AllProducts || x.ScopeType == OfferScopeType.Category)
                .WithMessage("ProductIds must be empty for AllProducts or Category scope.");

            RuleFor(x => x.CategoryIds)
                .Empty()
                .When(x => x.ScopeType == OfferScopeType.AllProducts || x.ScopeType == OfferScopeType.Product)
                .WithMessage("CategoryIds must be empty for AllProducts or Product scope.");

            RuleFor(x => x.ProductIds)
                .NotEmpty()
                .When(x => x.ScopeType == OfferScopeType.Product)
                .WithMessage("At least one ProductId is required for Product scope.");

            RuleFor(x => x.CategoryIds)
                .NotEmpty()
                .When(x => x.ScopeType == OfferScopeType.Category)
                .WithMessage("At least one CategoryId is required for Category scope.");

            // Prevent duplicates
            RuleFor(x => x.ProductIds)
                .Must(x => x == null || x.Distinct().Count() == x.Count)
                .WithMessage("ProductIds list contains duplicate IDs.");

            RuleFor(x => x.CategoryIds)
                .Must(x => x == null || x.Distinct().Count() == x.Count)
                .WithMessage("CategoryIds list contains duplicate IDs.");
        }
    }
}
