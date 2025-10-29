using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.VendorFeatures.Commands
{
    public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommandRequest, CreateVendorCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CreateVendorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<CreateVendorCommandResponse> Handle(CreateVendorCommandRequest request, CancellationToken cancellationToken)
        {
            var vendor = new Vendor
            {
                Name = request.Vendor.Name,
                ContactName = request.Vendor.ContactName,
                Phone = request.Vendor.Phone,
                Email = request.Vendor.Email,
                Address = request.Vendor.Address,
                TaxNumber = request.Vendor.TaxNumber,
                IsActive = request.Vendor.IsActive
            };
            
            await _unitOfWork.VendorRepository.AddAsync(vendor);
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
                CreatedAt = vendor.CreatedAt
            };
            
            return new CreateVendorCommandResponse
            {
                Success = true,
                Message = "Vendor created successfully",
                Vendor = vendorDto
            };
        }
    }
}