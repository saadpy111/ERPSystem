using MediatR;
using Procurement.Application.DTOs;
using System;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Queries
{
    public class GetPurchaseInvoiceByIdQueryRequest : IRequest<GetPurchaseInvoiceByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class GetPurchaseInvoiceByIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PurchaseInvoiceDto? PurchaseInvoice { get; set; }
    }
}