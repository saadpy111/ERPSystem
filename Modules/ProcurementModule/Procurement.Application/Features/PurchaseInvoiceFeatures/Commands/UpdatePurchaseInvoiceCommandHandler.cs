using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Commands
{
    public class UpdatePurchaseInvoiceCommandHandler : IRequestHandler<UpdatePurchaseInvoiceCommandRequest, UpdatePurchaseInvoiceCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdatePurchaseInvoiceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<UpdatePurchaseInvoiceCommandResponse> Handle(UpdatePurchaseInvoiceCommandRequest request, CancellationToken cancellationToken)
        {
            var purchaseInvoice = await _unitOfWork.PurchaseInvoiceRepository.GetByIdAsync(request.Id);
            
            if (purchaseInvoice == null)
            {
                return new UpdatePurchaseInvoiceCommandResponse
                {
                    Success = false,
                    Message = "Purchase invoice not found"
                };
            }
            
            // Update purchase invoice properties
            if (!string.IsNullOrEmpty(request.InvoiceNumber))
                purchaseInvoice.InvoiceNumber = request.InvoiceNumber;
                
            if (request.InvoiceDate.HasValue)
                purchaseInvoice.InvoiceDate = request.InvoiceDate.Value;
                
            if (request.TotalAmount.HasValue)
                purchaseInvoice.TotalAmount = request.TotalAmount.Value;
                
            if (!string.IsNullOrEmpty(request.PaymentStatus))
                purchaseInvoice.PaymentStatus = request.PaymentStatus;
                
            if (request.PaymentDate.HasValue)
                purchaseInvoice.PaymentDate = request.PaymentDate.Value;
                
            if (request.Notes != null)
                purchaseInvoice.Notes = request.Notes;
                
            purchaseInvoice.UpdatedAt = System.DateTime.UtcNow;
            
            _unitOfWork.PurchaseInvoiceRepository.Update(purchaseInvoice);
            await _unitOfWork.SaveChangesAsync();
            
            return new UpdatePurchaseInvoiceCommandResponse
            {
                Success = true,
                Message = "Purchase invoice updated successfully"
            };
        }
    }
}