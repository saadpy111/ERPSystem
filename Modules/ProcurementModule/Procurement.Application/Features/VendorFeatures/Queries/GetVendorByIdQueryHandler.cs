using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Procurement.Application.DTOs.AttachmentDtos;
using Procurement.Domain.Entities;

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

            // Get Attachments related to this Vendor (Polymorphic)
            var attachments = await _unitOfWork.ProcurementAttachmentRepository.GetAllByEntityAsync(nameof(Vendor), vendor.Id);

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
                Rate = vendor.Rate,
                VendorCode = vendor.VendorCode,
                ProductVendorName = vendor.ProductVendorName,
                Webpage = vendor.Webpage,
                Govrenment = vendor.Govrenment,
                City = vendor.City,
                Currency = vendor.Currency,
                PaymentMethod = vendor.PaymentMethod,
                CommercialRegistrationNumber = vendor.CommercialRegistrationNumber,
                SupplierCreditLimit = vendor.SupplierCreditLimit,
                Notes = vendor.Notes,
                IsActive = vendor.IsActive,
                CreatedAt = vendor.CreatedAt,
                UpdatedAt = vendor.UpdatedAt
            };

            if (attachments != null && attachments.Any())
            {
                vendorDto.Attachments = attachments.Select(a => new AttachmentDto
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    FileUrl = a.FileUrl,
                    ContentType = a.ContentType,
                    Description = a.Description,
                    UploadedAt = a.UploadedAt
                }).ToList();
            }
            else
            {
                vendorDto.Attachments = new List<AttachmentDto>();
            }

            return new GetVendorByIdQueryResponse
            {
                Success = true,
                Vendor = vendorDto
            };
        }
    }
}