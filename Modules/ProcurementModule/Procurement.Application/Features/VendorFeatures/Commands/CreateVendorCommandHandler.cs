using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Procurement.Application.Contracts.Infrastructure.FileService;

namespace Procurement.Application.Features.VendorFeatures.Commands
{
    public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommandRequest, CreateVendorCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcurementFileService _fileService;

        public CreateVendorCommandHandler(IUnitOfWork unitOfWork, IProcurementFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
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
                Rate = request.Vendor.Rate,
                VendorCode = request.Vendor.VendorCode,
                ProductVendorName = request.Vendor.ProductVendorName,
                Webpage = request.Vendor.Webpage,
                Govrenment = request.Vendor.Govrenment,
                City = request.Vendor.City,
                Currency = request.Vendor.Currency,
                PaymentMethod = request.Vendor.PaymentMethod,
                CommercialRegistrationNumber = request.Vendor.CommercialRegistrationNumber,
                SupplierCreditLimit = request.Vendor.SupplierCreditLimit,
                Notes = request.Vendor.Notes,
                IsActive = request.Vendor.IsActive
            };

            await _unitOfWork.VendorRepository.AddAsync(vendor);
            await _unitOfWork.SaveChangesAsync();

            // attachments: save files and create attachment records
            if (request.Vendor.Attachments != null && request.Vendor.Attachments.Any())
            {
                foreach (var attachmentDto in request.Vendor.Attachments)
                {
                    var filePath = await _fileService.SaveFileAsync(attachmentDto.File, "vendorAttachments");
                    var attachment = new ProcurementAttachment
                    {
                        FileName = attachmentDto.File.FileName,
                        FileUrl = filePath,
                        ContentType = attachmentDto.File.ContentType,
                        FileSize = attachmentDto.File.Length,
                        EntityType = nameof(Vendor),
                        EntityId = vendor.Id,
                        Description = attachmentDto.Description,
                        UploadedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.ProcurementAttachmentRepository.AddAsync(attachment);
                }
                await _unitOfWork.SaveChangesAsync();
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