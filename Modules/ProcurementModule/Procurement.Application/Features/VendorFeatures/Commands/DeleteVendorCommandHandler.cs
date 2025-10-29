using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.VendorFeatures.Commands
{
    public class DeleteVendorCommandHandler : IRequestHandler<DeleteVendorCommandRequest, DeleteVendorCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DeleteVendorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<DeleteVendorCommandResponse> Handle(DeleteVendorCommandRequest request, CancellationToken cancellationToken)
        {
            var vendor = await _unitOfWork.VendorRepository.GetByIdAsync(request.Id);
            
            if (vendor == null)
            {
                return new DeleteVendorCommandResponse
                {
                    Success = false,
                    Message = "Vendor not found"
                };
            }
            
            _unitOfWork.VendorRepository.Delete(vendor);
            await _unitOfWork.SaveChangesAsync();
            
            return new DeleteVendorCommandResponse
            {
                Success = true,
                Message = "Vendor deleted successfully"
            };
        }
    }
}