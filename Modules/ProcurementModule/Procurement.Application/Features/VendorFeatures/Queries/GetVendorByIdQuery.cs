using MediatR;
using Procurement.Application.DTOs;
using System;

namespace Procurement.Application.Features.VendorFeatures.Queries
{
    public class GetVendorByIdQueryRequest : IRequest<GetVendorByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class GetVendorByIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public VendorDto? Vendor { get; set; }
    }
}