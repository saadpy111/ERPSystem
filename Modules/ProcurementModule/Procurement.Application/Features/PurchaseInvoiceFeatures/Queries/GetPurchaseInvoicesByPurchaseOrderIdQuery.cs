using MediatR;
using Procurement.Application.DTOs;
using System;
using System.Collections.Generic;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Queries
{
    public class GetPurchaseInvoicesByPurchaseOrderIdQueryRequest : IRequest<GetPurchaseInvoicesByPurchaseOrderIdQueryResponse>
    {
        public Guid PurchaseOrderId { get; set; }
    }
    
    public class GetPurchaseInvoicesByPurchaseOrderIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PurchaseInvoiceDto> PurchaseInvoices { get; set; } = new List<PurchaseInvoiceDto>();
    }
}