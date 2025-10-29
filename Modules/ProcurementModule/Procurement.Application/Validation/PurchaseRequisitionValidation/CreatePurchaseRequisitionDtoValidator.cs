using FluentValidation;
using Procurement.Application.DTOs;

namespace Procurement.Application.Validation.PurchaseRequisitionValidation
{
    public class CreatePurchaseRequisitionDtoValidator : AbstractValidator<CreatePurchaseRequisitionDto>
    {
        public CreatePurchaseRequisitionDtoValidator()
        {
            RuleFor(x => x.RequestedBy)
                .NotEmpty().WithMessage("Requested by is required")
                .MaximumLength(100).WithMessage("Requested by must not exceed 100 characters");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .MaximumLength(50).WithMessage("Status must not exceed 50 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes must not exceed 500 characters");
        }
    }
}