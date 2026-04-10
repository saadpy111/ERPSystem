using Accounting.Application.Features.Vouchers.Commands.CreateVoucher;
using Accounting.Application.Features.Vouchers.DTOs;
using Accounting.Application.Interfaces.Repositories;
using FluentValidation;
using System.Linq;

namespace Accounting.Application.Features.Vouchers.Validators
{
    public class CreateVoucherCommandValidator : AbstractValidator<CreateVoucherCommand>
    {
        public CreateVoucherCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(v => v.VoucherType).NotEmpty();
            RuleFor(v => v.Date).NotEmpty();
            RuleFor(v => v.CurrencyId).NotEmpty();
            RuleFor(v => v.Lines).NotEmpty().WithMessage("At least one line exists");
            RuleForEach(v => v.Lines).SetValidator(new VoucherLineValidator(unitOfWork));
            
            RuleFor(v => v.Lines)
                .Must(lines => lines.Sum(l => l.Debit) == lines.Sum(l => l.Credit))
                .WithMessage("Total Debit must equal Total Credit");

            RuleFor(v => v.Lines)
                .Must(lines => lines.Sum(l => l.Debit) > 0 || lines.Sum(l => l.Credit) > 0)
                .WithMessage("Total amount must be greater than zero");
        }
    }

    public class VoucherLineValidator : AbstractValidator<CreateVoucherLineDto>
    {
        public VoucherLineValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(v => v.AccountId).NotEmpty();
            RuleFor(v => v.Debit).GreaterThanOrEqualTo(0).WithMessage("Debit cannot be negative");
            RuleFor(v => v.Credit).GreaterThanOrEqualTo(0).WithMessage("Credit cannot be negative");
            RuleFor(v => v.CurrencyId).NotEmpty();
            
            RuleFor(v => v.CostCenterId)
                .MustAsync(async (ccId, ct) => 
                {
                    if (!ccId.HasValue) return true;
                    var cc = await unitOfWork.CostCenters.GetByIdAsync(ccId.Value);
                    return cc != null && cc.IsActive;
                })
                .WithMessage("Cost Center must exist and be active.");

            RuleFor(v => v)
                .Must(v => v.Debit > 0 || v.Credit > 0)
                .WithMessage("Either Debit or Credit must be greater than zero");

            RuleFor(v => v)
                .Must(v => !(v.Debit > 0 && v.Credit > 0))
                .WithMessage("Line cannot have both Debit and Credit");
        }
    }
}
