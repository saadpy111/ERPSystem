using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Partners.Commands.TogglePartnerStatus
{
    public class TogglePartnerStatusCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class TogglePartnerStatusCommandHandler : IRequestHandler<TogglePartnerStatusCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TogglePartnerStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(TogglePartnerStatusCommand request, CancellationToken cancellationToken)
        {
            var partner = await _unitOfWork.Partners.GetByIdAsync(request.Id);
            if (partner == null)
                return Result.Failure("Partner not found.");

            partner.IsActive = !partner.IsActive;
            
            _unitOfWork.Partners.Update(partner);
            await _unitOfWork.SaveChangesAsync();

            string status = partner.IsActive ? "activated" : "deactivated";
            return Result.Ok($"Partner {status} successfully.");
        }
    }
}
