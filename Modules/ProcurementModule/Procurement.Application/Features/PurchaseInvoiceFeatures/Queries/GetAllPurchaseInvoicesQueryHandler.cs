using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Queries
{
    public class GetAllPurchaseInvoicesQueryHandler : IRequestHandler<GetAllPurchaseInvoicesQueryRequest, GetAllPurchaseInvoicesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetAllPurchaseInvoicesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetAllPurchaseInvoicesQueryResponse> Handle(GetAllPurchaseInvoicesQueryRequest request, CancellationToken cancellationToken)
        {
            var purchaseInvoices = await _unitOfWork.PurchaseInvoiceRepository.GetAllAsync();
            
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
            
            return new GetAllPurchaseInvoicesQueryResponse
            {
                Success = true,
                PurchaseInvoices = purchaseInvoiceDtos
            };
        }
    }
}