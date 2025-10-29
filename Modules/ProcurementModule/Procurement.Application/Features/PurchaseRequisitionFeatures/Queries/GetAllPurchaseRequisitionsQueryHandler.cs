using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseRequisitionFeatures.Queries
{
    public class GetAllPurchaseRequisitionsQueryHandler : IRequestHandler<GetAllPurchaseRequisitionsQueryRequest, GetAllPurchaseRequisitionsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetAllPurchaseRequisitionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetAllPurchaseRequisitionsQueryResponse> Handle(GetAllPurchaseRequisitionsQueryRequest request, CancellationToken cancellationToken)
        {
            var purchaseRequisitions = await _unitOfWork.PurchaseRequisitionRepository.GetAllAsync();
            
            // Map to DTOs
            var purchaseRequisitionDtos = purchaseRequisitions.Select(purchaseRequisition => new PurchaseRequisitionDto
            {
                Id = purchaseRequisition.Id,
                RequestedBy = purchaseRequisition.RequestedBy,
                RequestDate = purchaseRequisition.RequestDate,
                Status = purchaseRequisition.Status,
                Notes = purchaseRequisition.Notes,
                CreatedAt = purchaseRequisition.CreatedAt,
                UpdatedAt = purchaseRequisition.UpdatedAt
            }).ToList();
            
            return new GetAllPurchaseRequisitionsQueryResponse
            {
                Success = true,
                PurchaseRequisitions = purchaseRequisitionDtos
            };
        }
    }
}