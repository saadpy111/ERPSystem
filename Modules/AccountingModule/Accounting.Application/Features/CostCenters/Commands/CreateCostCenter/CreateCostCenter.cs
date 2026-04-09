using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Entities;
using FluentValidation;
using MediatR;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.CostCenters.Commands.CreateCostCenter
{
    public class CreateCostCenterCommand : IRequest<Result<int>>
    {
        public string NameAr { get; set; } = null!;
    }

    public class CreateCostCenterCommandValidator : AbstractValidator<CreateCostCenterCommand>
    {
        public CreateCostCenterCommandValidator()
        {
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(100);
        }
    }

    public class CreateCostCenterCommandHandler : IRequestHandler<CreateCostCenterCommand, Result<int>>
    {
        private readonly IAccountingDbContext _context;

        public CreateCostCenterCommandHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(CreateCostCenterCommand request, CancellationToken cancellationToken)
        {
            var cc = new CostCenter
            {
                Name = request.NameAr,
                IsActive = true
            };

            await _context.CostCenters.AddAsync(cc, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return cc.Id;
        }
    }
}
