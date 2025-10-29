using FluentValidation;
using Procurement.Application.DTOs;

namespace Procurement.Application.Validation.GoodsReceiptValidation
{
    public class CreateGoodsReceiptDtoValidator : AbstractValidator<CreateGoodsReceiptDto>
    {
        public CreateGoodsReceiptDtoValidator()
        {
            RuleFor(x => x.PurchaseOrderId)
                .NotEmpty().WithMessage("Purchase order ID is required");

            //RuleFor(x => x.ReceivedBy)
            //    .NotEmpty().WithMessage("Received by is required")
            //    .MaximumLength(100).WithMessage("Received by must not exceed 100 characters");

            //RuleFor(x => x.Status)
            //    .NotEmpty().WithMessage("Status is required")
            //    .MaximumLength(50).WithMessage("Status must not exceed 50 characters");

            RuleFor(x => x.Remarks)
                .MaximumLength(500).WithMessage("Remarks must not exceed 500 characters");
        }
    }
}