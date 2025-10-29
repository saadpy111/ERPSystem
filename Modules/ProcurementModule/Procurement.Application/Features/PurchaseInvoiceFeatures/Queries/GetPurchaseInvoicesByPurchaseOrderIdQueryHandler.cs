using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Queries
{
    public class GetPurchaseInvoicesByPurchaseOrderIdQueryHandler : IRequestHandler<GetPurchaseInvoicesByPurchaseOrderIdQueryRequest, GetPurchaseInvoicesByPurchaseOrderIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetPurchaseInvoicesByPurchaseOrderIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetPurchaseInvoicesByPurchaseOrderIdQueryResponse> Handle(GetPurchaseInvoicesByPurchaseOrderIdQueryRequest request, CancellationToken cancellationToken)
        {
            var purchaseInvoices = await _unitOfWork.PurchaseInvoiceRepository.GetByPurchaseOrderIdAsync(request.PurchaseOrderId);
            
            // Map to DTOs
            var purchaseInvoiceDtos = purchaseInvoices.Select(purchaseInvoice => new PurchaseInvoiceDto
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
            }).ToList();
            
            return new GetPurchaseInvoicesByPurchaseOrderIdQueryResponse
            {
                Success = true,
                PurchaseInvoices = purchaseInvoiceDtos
            };
        }
    }
}