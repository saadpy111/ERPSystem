using MediatR;
using Procurement.Application.DTOs;
using System.Collections.Generic;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Queries
{
    public class GetAllPurchaseInvoicesQueryRequest : IRequest<GetAllPurchaseInvoicesQueryResponse>
    {
    }
    
    public class GetAllPurchaseInvoicesQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PurchaseInvoiceDto> PurchaseInvoices { get; set; } = new List<PurchaseInvoiceDto>();
    }
}