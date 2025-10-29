using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Commands
{
    public class DeletePurchaseInvoiceCommandHandler : IRequestHandler<DeletePurchaseInvoiceCommandRequest, DeletePurchaseInvoiceCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DeletePurchaseInvoiceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<DeletePurchaseInvoiceCommandResponse> Handle(DeletePurchaseInvoiceCommandRequest request, CancellationToken cancellationToken)
        {
            var purchaseInvoice = await _unitOfWork.PurchaseInvoiceRepository.GetByIdAsync(request.Id);
            
            if (purchaseInvoice == null)
            {
                return new DeletePurchaseInvoiceCommandResponse
                {
                    Success = false,
                    Message = "Purchase invoice not found"
                };
            }
            
            _unitOfWork.PurchaseInvoiceRepository.Delete(purchaseInvoice);
            await _unitOfWork.SaveChangesAsync();
            
            return new DeletePurchaseInvoiceCommandResponse
            {
                Success = true,
                Message = "Purchase invoice deleted successfully"
            };
        }
    }
}