using FluentValidation;
using Procurement.Application.DTOs;

namespace Procurement.Application.Validation.PurchaseInvoiceValidation
{
    public class CreatePurchaseInvoiceDtoValidator : AbstractValidator<CreatePurchaseInvoiceDto>
    {
        public CreatePurchaseInvoiceDtoValidator()
        {
            RuleFor(x => x.PurchaseOrderId)
                .NotEmpty().WithMessage("Purchase order ID is required");

            RuleFor(x => x.InvoiceNumber)
                .NotEmpty().WithMessage("Invoice number is required")
                .MaximumLength(100).WithMessage("Invoice number must not exceed 100 characters");

            RuleFor(x => x.PaymentStatus)
                .NotEmpty().WithMessage("Payment status is required")
                .MaximumLength(50).WithMessage("Payment status must not exceed 50 characters");

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount must be greater than or equal to zero");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes must not exceed 500 characters");
        }
    }
}