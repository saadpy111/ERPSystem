using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.VendorFeatures.Queries
{
    public class GetVendorByIdQueryHandler : IRequestHandler<GetVendorByIdQueryRequest, GetVendorByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetVendorByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetVendorByIdQueryResponse> Handle(GetVendorByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var vendor = await _unitOfWork.VendorRepository.GetByIdAsync(request.Id);
            
            if (vendor == null)
            {
                return new GetVendorByIdQueryResponse
                {
                    Success = false,
                    Message = "Vendor not found"
                };
            }
            
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
            
            return new GetVendorByIdQueryResponse
            {
                Success = true,
                Vendor = vendorDto
            };
        }
    }
}