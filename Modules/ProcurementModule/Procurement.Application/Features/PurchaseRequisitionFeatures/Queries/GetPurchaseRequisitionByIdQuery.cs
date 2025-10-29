using MediatR;
using Procurement.Application.DTOs;
using System;

namespace Procurement.Application.Features.PurchaseRequisitionFeatures.Queries
{
    public class GetPurchaseRequisitionByIdQueryRequest : IRequest<GetPurchaseRequisitionByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class GetPurchaseRequisitionByIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PurchaseRequisitionDto? PurchaseRequisition { get; set; }
    }
}