using MediatR;
using Procurement.Application.DTOs;
using System.Collections.Generic;

namespace Procurement.Application.Features.PurchaseRequisitionFeatures.Queries
{
    public class GetAllPurchaseRequisitionsQueryRequest : IRequest<GetAllPurchaseRequisitionsQueryResponse>
    {
    }
    
    public class GetAllPurchaseRequisitionsQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PurchaseRequisitionDto> PurchaseRequisitions { get; set; } = new List<PurchaseRequisitionDto>();
    }
}