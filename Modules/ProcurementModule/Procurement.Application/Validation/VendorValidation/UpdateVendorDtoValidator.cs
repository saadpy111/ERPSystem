using FluentValidation;
using Procurement.Application.DTOs;

namespace Procurement.Application.Validation.VendorValidation
{
    public class UpdateVendorDtoValidator : AbstractValidator<UpdateVendorDto>
    {
        public UpdateVendorDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Vendor ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Vendor name is required")
                .MaximumLength(200).WithMessage("Vendor name must not exceed 200 characters");

            RuleFor(x => x.ContactName)
                .MaximumLength(200).WithMessage("Contact name must not exceed 200 characters");

            RuleFor(x => x.Email)
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters")
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email format");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters");

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("Address must not exceed 500 characters");

            RuleFor(x => x.TaxNumber)
                .MaximumLength(50).WithMessage("Tax number must not exceed 50 characters");
        }
    }
}