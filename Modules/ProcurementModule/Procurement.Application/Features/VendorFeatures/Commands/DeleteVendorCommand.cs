using MediatR;
using System;

namespace Procurement.Application.Features.VendorFeatures.Commands
{
    public class DeleteVendorCommandRequest : IRequest<DeleteVendorCommandResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteVendorCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}