using MediatR;
using Procurement.Application.DTOs;

namespace Procurement.Application.Features.VendorFeatures.Commands
{
    public class CreateVendorCommandRequest : IRequest<CreateVendorCommandResponse>
    {
        public CreateVendorDto Vendor { get; set; } = null!;
    }
    
    public class CreateVendorCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public VendorDto? Vendor { get; set; }
    }
}