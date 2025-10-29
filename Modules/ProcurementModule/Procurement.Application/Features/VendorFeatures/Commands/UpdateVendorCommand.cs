using MediatR;
using Procurement.Application.DTOs;

namespace Procurement.Application.Features.VendorFeatures.Commands
{
    public class UpdateVendorCommandRequest : IRequest<UpdateVendorCommandResponse>
    {
        public UpdateVendorDto Vendor { get; set; } = null!;
    }
    
    public class UpdateVendorCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public VendorDto? Vendor { get; set; }
    }
}