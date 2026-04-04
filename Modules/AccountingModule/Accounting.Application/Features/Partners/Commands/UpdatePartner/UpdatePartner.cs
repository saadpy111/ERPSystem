using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Partners.Commands.UpdatePartner
{
    public class UpdatePartnerCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = null!;
    }

    public class UpdatePartnerCommandValidator : AbstractValidator<UpdatePartnerCommand>
    {
        public UpdatePartnerCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(100);
        }
    }

    public class UpdatePartnerCommandHandler : IRequestHandler<UpdatePartnerCommand, Unit>
    {
        private readonly IAccountingDbContext _context;

        public UpdatePartnerCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePartnerCommand request, CancellationToken cancellationToken)
        {
            var partner = await _context.Partners.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (partner == null) throw new BusinessException("Not found");

            partner.NameAr = request.NameAr;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
