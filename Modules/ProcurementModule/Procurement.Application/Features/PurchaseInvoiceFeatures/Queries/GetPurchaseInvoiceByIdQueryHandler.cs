using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Queries
{
    public class GetPurchaseInvoiceByIdQueryHandler : IRequestHandler<GetPurchaseInvoiceByIdQueryRequest, GetPurchaseInvoiceByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetPurchaseInvoiceByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetPurchaseInvoiceByIdQueryResponse> Handle(GetPurchaseInvoiceByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var purchaseInvoice = await _unitOfWork.PurchaseInvoiceRepository.GetByIdAsync(request.Id);
            
            if (purchaseInvoice == null)
            {
                return new GetPurchaseInvoiceByIdQueryResponse
                {
                    Success = false,
                    Message = "Purchase invoice not found"
                };
            }
            
            // Map to DTO
            var purchaseInvoiceDto = new PurchaseInvoiceDto
            {
                Id = purchaseInvoice.Id,
                PurchaseOrderId = purchaseInvoice.PurchaseOrderId,
                InvoiceNumber = purchaseInvoice.InvoiceNumber,
                InvoiceDate = purchaseInvoice.InvoiceDate,
                TotalAmount = purchaseInvoice.TotalAmount,
                PaymentStatus = purchaseInvoice.PaymentStatus,
                PaymentDate = purchaseInvoice.PaymentDate,
                Notes = purchaseInvoice.Notes,
                CreatedAt = purchaseInvoice.CreatedAt,
                UpdatedAt = purchaseInvoice.UpdatedAt
            };
            
            return new GetPurchaseInvoiceByIdQueryResponse
            {
                Success = true,
                PurchaseInvoice = purchaseInvoiceDto
            };
        }
    }
}