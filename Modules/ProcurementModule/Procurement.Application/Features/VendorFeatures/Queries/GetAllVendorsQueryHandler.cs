using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.VendorFeatures.Queries
{
    public class GetAllVendorsQueryHandler : IRequestHandler<GetAllVendorsQueryRequest, GetAllVendorsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetAllVendorsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetAllVendorsQueryResponse> Handle(GetAllVendorsQueryRequest request, CancellationToken cancellationToken)
        {
            var vendors = await _unitOfWork.VendorRepository.GetAllAsync();
            
            // Map to DTOs
            var vendorDtos = vendors.Select(vendor => new VendorDto
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
            }).ToList();
            
            return new GetAllVendorsQueryResponse
            {
                Success = true,
                Vendors = vendorDtos
            };
        }
    }
}