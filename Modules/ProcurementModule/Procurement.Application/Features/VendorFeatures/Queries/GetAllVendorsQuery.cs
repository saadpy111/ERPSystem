using MediatR;
using Procurement.Application.DTOs;
using System.Collections.Generic;

namespace Procurement.Application.Features.VendorFeatures.Queries
{
    public class GetAllVendorsQueryRequest : IRequest<GetAllVendorsQueryResponse>
    {
    }
    
    public class GetAllVendorsQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<VendorDto> Vendors { get; set; } = new List<VendorDto>();
    }
}