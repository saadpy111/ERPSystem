using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseRequisitionFeatures.Commands
{
    public class CreatePurchaseRequisitionCommandHandler : IRequestHandler<CreatePurchaseRequisitionCommandRequest, CreatePurchaseRequisitionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CreatePurchaseRequisitionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<CreatePurchaseRequisitionCommandResponse> Handle(CreatePurchaseRequisitionCommandRequest request, CancellationToken cancellationToken)
        {
            var purchaseRequisition = new PurchaseRequisition
            {
                RequestedBy = request.PurchaseRequisition.RequestedBy,
                RequestDate = request.PurchaseRequisition.RequestDate,
                Status = request.PurchaseRequisition.Status,
                Notes = request.PurchaseRequisition.Notes
            };
            
            await _unitOfWork.PurchaseRequisitionRepository.AddAsync(purchaseRequisition);
            await _unitOfWork.SaveChangesAsync();
            
            // Map to DTO
            var purchaseRequisitionDto = new PurchaseRequisitionDto
            {
                Id = purchaseRequisition.Id,
                RequestedBy = purchaseRequisition.RequestedBy,
                RequestDate = purchaseRequisition.RequestDate,
                Status = purchaseRequisition.Status,
                Notes = purchaseRequisition.Notes,
                CreatedAt = purchaseRequisition.CreatedAt
            };
            
            return new CreatePurchaseRequisitionCommandResponse
            {
                Success = true,
                Message = "Purchase requisition created successfully",
                PurchaseRequisition = purchaseRequisitionDto
            };
        }
    }
}