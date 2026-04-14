using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Partners.Commands.DeletePartner
{
    public class DeletePartnerCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeletePartnerCommandHandler : IRequestHandler<DeletePartnerCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePartnerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeletePartnerCommand request, CancellationToken cancellationToken)
        {
            var partner = await _unitOfWork.Partners.GetByIdAsync(request.Id);
            if (partner == null)
                return Result.Failure("Partner not found.");

            // Soft delete
            partner.IsDeleted = true;
            _unitOfWork.Partners.Update(partner);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok("Partner deleted successfully.");
        }
    }
}
