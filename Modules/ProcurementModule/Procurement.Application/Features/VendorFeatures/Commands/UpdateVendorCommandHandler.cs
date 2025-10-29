using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.VendorFeatures.Commands
{
    public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommandRequest, UpdateVendorCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdateVendorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<UpdateVendorCommandResponse> Handle(UpdateVendorCommandRequest request, CancellationToken cancellationToken)
        {
            var vendor = await _unitOfWork.VendorRepository.GetByIdAsync(request.Vendor.Id);
            
            if (vendor == null)
            {
                return new UpdateVendorCommandResponse
                {
                    Success = false,
                    Message = "Vendor not found"
                };
            }
            
            // Update vendor properties
            vendor.Name = request.Vendor.Name;
            vendor.ContactName = request.Vendor.ContactName;
            vendor.Phone = request.Vendor.Phone;
            vendor.Email = request.Vendor.Email;
            vendor.Address = request.Vendor.Address;
            vendor.TaxNumber = request.Vendor.TaxNumber;
            vendor.IsActive = request.Vendor.IsActive;
            vendor.UpdatedAt = System.DateTime.UtcNow;
            
            _unitOfWork.VendorRepository.Update(vendor);
            await _unitOfWork.SaveChangesAsync();
            
            // Map to DTO
            var vendorDto = new VendorDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                ContactName = vendor.ContactName,
                Phone = vendor.Phone,
                Email = vendor.Email,
                Address = vendor.Address,
                TaxNumber = vendor.TaxNumber,
                IsActive = vendor.IsActive,
                CreatedAt = vendor.CreatedAt,
                UpdatedAt = vendor.UpdatedAt
            };
            
            return new UpdateVendorCommandResponse
            {
                Success = true,
                Message = "Vendor updated successfully",
                Vendor = vendorDto
            };
        }
    }
}