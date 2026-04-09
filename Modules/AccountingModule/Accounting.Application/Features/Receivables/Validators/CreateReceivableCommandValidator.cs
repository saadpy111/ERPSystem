using Accounting.Application.Features.Receivables.Commands;
using Accounting.Application.Interfaces.Repositories;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Receivables.Validators
{
    public class CreateReceivableCommandValidator : AbstractValidator<CreateReceivableCommand>
    {
        private readonly IUnitOfWork _uow;

        public CreateReceivableCommandValidator(IUnitOfWork uow)
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
