using Accounting.Application.Features.Payables.Commands;
using Accounting.Application.Interfaces.Repositories;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Payables.Validators
{
    public class CreatePayableCommandValidator : AbstractValidator<CreatePayableCommand>
    {
        private readonly IUnitOfWork _uow;

        public CreatePayableCommandValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero");

            RuleFor(x => x.PartnerId)
                .MustAsync(PartnerExists)
                .WithMessage("Partner not found");
        }

        private async Task<bool> PartnerExists(int partnerId, CancellationToken cancellationToken)
        {
            var partner = await _uow.Partners.GetByIdAsync(partnerId);
            return partner != null;
        }
    }
}
