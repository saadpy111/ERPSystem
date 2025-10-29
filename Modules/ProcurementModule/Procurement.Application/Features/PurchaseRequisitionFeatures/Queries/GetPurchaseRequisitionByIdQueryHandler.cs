using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseRequisitionFeatures.Queries
{
    public class GetPurchaseRequisitionByIdQueryHandler : IRequestHandler<GetPurchaseRequisitionByIdQueryRequest, GetPurchaseRequisitionByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetPurchaseRequisitionByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetPurchaseRequisitionByIdQueryResponse> Handle(GetPurchaseRequisitionByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var purchaseRequisition = await _unitOfWork.PurchaseRequisitionRepository.GetByIdAsync(request.Id);
            
            if (purchaseRequisition == null)
            {
                return new GetPurchaseRequisitionByIdQueryResponse
                {
                    Success = false,
                    Message = "Purchase requisition not found"
                };
            }
            
            // Map to DTO
            var purchaseRequisitionDto = new PurchaseRequisitionDto
            {
                Id = purchaseRequisition.Id,
                RequestedBy = purchaseRequisition.RequestedBy,
                RequestDate = purchaseRequisition.RequestDate,
                Status = purchaseRequisition.Status,
                Notes = purchaseRequisition.Notes,
                CreatedAt = purchaseRequisition.CreatedAt,
                UpdatedAt = purchaseRequisition.UpdatedAt
            };
            
            return new GetPurchaseRequisitionByIdQueryResponse
            {
                Success = true,
                PurchaseRequisition = purchaseRequisitionDto
            };
        }
    }
}