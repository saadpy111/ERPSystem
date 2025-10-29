using FluentValidation;
using Procurement.Application.DTOs;

namespace Procurement.Application.Validation.PurchaseOrderValidation
{
    public class CreatePurchaseOrderDtoValidator : AbstractValidator<CreatePurchaseOrderDto>
    {
        public CreatePurchaseOrderDtoValidator()
        {
            RuleFor(x => x.VendorId)
                .NotEmpty().WithMessage("Vendor ID is required");

            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("Created by is required")
                .MaximumLength(100).WithMessage("Created by must not exceed 100 characters");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Purchase order items are required")
                .Must(items => items.Count > 0).WithMessage("At least one purchase order item is required");

            RuleForEach(x => x.Items)
                .SetValidator(new CreatePurchaseOrderItemDtoValidator());
        }
    }
    
    public class CreatePurchaseOrderItemDtoValidator : AbstractValidator<CreatePurchaseOrderItemDto>
    {
        public CreatePurchaseOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Unit price must be greater than or equal to zero");
        }
    }
}