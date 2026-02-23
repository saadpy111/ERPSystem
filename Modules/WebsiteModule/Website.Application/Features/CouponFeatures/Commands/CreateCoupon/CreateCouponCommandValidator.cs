using FluentValidation;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Features.CouponFeatures.Commands.CreateCoupon
{
    public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommandRequest>
    {
        public CreateCouponCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters.")
                .MustAsync(async (code, cancellation) =>
                {
                    var exists = await unitOfWork.Repository<Coupon>().AnyAsync(c => c.Code == code);
                    return !exists;
                }).WithMessage("Coupon code already exists.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("Discount value must be greater than 0.");

            RuleFor(x => x.DiscountValue)
                .InclusiveBetween(0.01m, 100m)
                .When(x => x.DiscountType == DiscountType.Percentage)
                .WithMessage("Percentage discount must be between 0.01 and 100.");

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate).WithMessage("Start date must be before end date.");

            RuleFor(x => x.MinimumOrderAmount)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinimumOrderAmount.HasValue)
                .WithMessage("Minimum order amount must be 0 or greater.");

            RuleFor(x => x.UsageLimit)
                .GreaterThanOrEqualTo(0)
                .When(x => x.UsageLimit.HasValue)
                .WithMessage("Usage limit must be 0 or greater.");

            RuleFor(x => x.UsagePerUserLimit)
                .GreaterThanOrEqualTo(0)
                .When(x => x.UsagePerUserLimit.HasValue)
                .WithMessage("Usage per user limit must be 0 or greater.");
        }
    }
}
