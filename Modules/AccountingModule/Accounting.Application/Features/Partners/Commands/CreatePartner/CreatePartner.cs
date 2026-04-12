using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Entities;
using FluentValidation;
using MediatR;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Partners.Commands.CreatePartner
{
    public class CreatePartnerCommand : IRequest<Result<int>>
    {
        public string NameAr { get; set; } = null!;
    }

    public class CreatePartnerCommandValidator : AbstractValidator<CreatePartnerCommand>
    {
        public CreatePartnerCommandValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(100);
        }
    }

    public class CreatePartnerCommandHandler : IRequestHandler<CreatePartnerCommand, Result<int>>
    {
        private readonly IAccountingDbContext _context;

        public CreatePartnerCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(CreatePartnerCommand request, CancellationToken cancellationToken)
        {
            var partner = new Partner
            {
                  
                NameAr = request.NameAr,
                IsActive = true
            };
            
            await _context.Partners.AddAsync(partner, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return partner.Id;
        }
    }
}
