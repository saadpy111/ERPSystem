using FluentValidation;
using Procurement.Application.DTOs;
using Procurement.Application.Features.PurchaseOrderFeatures.Commands;

namespace Procurement.Application.Validation.PurchaseOrderValidation
{
    public class UpdatePurchaseOrderDtoValidator : AbstractValidator<UpdatePurchaseOrderCommandRequest>
    {
        public UpdatePurchaseOrderDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Purchase order ID is required");

            RuleFor(x => x.VendorId)
                .NotEmpty().When(x => x.VendorId.HasValue)
                .WithMessage("Vendor ID must be valid");

            RuleFor(x => x.CreatedBy)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.CreatedBy))
                .WithMessage("Created by must not exceed 100 characters");

            RuleFor(x => x.Status)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Status))
                .WithMessage("Status must not exceed 50 characters");
        }
    }
}