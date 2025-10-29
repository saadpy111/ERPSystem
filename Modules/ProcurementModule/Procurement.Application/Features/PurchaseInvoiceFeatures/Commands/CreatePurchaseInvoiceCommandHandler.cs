using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Commands
{
    public class CreatePurchaseInvoiceCommandHandler : IRequestHandler<CreatePurchaseInvoiceCommandRequest, CreatePurchaseInvoiceCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CreatePurchaseInvoiceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<CreatePurchaseInvoiceCommandResponse> Handle(CreatePurchaseInvoiceCommandRequest request, CancellationToken cancellationToken)
        {
            var purchaseInvoice = new PurchaseInvoice
            {
                PurchaseOrderId = request.PurchaseInvoice.PurchaseOrderId,
                InvoiceNumber = request.PurchaseInvoice.InvoiceNumber,
                InvoiceDate = request.PurchaseInvoice.InvoiceDate,
                TotalAmount = request.PurchaseInvoice.TotalAmount,
                PaymentStatus = request.PurchaseInvoice.PaymentStatus,
                PaymentDate = request.PurchaseInvoice.PaymentDate,
                Notes = request.PurchaseInvoice.Notes
            };
            
            await _unitOfWork.PurchaseInvoiceRepository.AddAsync(purchaseInvoice);
            await _unitOfWork.SaveChangesAsync();
            
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
                CreatedAt = purchaseInvoice.CreatedAt
            };
            
            return new CreatePurchaseInvoiceCommandResponse
            {
                Success = true,
                Message = "Purchase invoice created successfully",
                PurchaseInvoice = purchaseInvoiceDto
            };
        }
    }
}