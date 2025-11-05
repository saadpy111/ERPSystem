using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Procurement.Application.Contracts.Infrastructure.FileService;
using Procurement.Domain.Entities;

namespace Procurement.Application.Features.VendorFeatures.Commands
{
    public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommandRequest, UpdateVendorCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcurementFileService _fileService;

        public UpdateVendorCommandHandler(IUnitOfWork unitOfWork, IProcurementFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
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
            vendor.Rate = request.Vendor.Rate;
            vendor.VendorCode = request.Vendor.VendorCode;
            vendor.ProductVendorName = request.Vendor.ProductVendorName;
            vendor.Webpage = request.Vendor.Webpage;
            vendor.Govrenment = request.Vendor.Govrenment;
            vendor.City = request.Vendor.City;
            vendor.Currency = request.Vendor.Currency;
            vendor.PaymentMethod = request.Vendor.PaymentMethod;
            vendor.CommercialRegistrationNumber = request.Vendor.CommercialRegistrationNumber;
            vendor.SupplierCreditLimit = request.Vendor.SupplierCreditLimit;
            vendor.Notes = request.Vendor.Notes;
            vendor.IsActive = request.Vendor.IsActive;
            vendor.UpdatedAt = System.DateTime.UtcNow;

            // update attachments
            if (request.Vendor.EditAttachments)
            {
                var oldAttachments = await _unitOfWork.ProcurementAttachmentRepository.GetAllByEntityAsync(nameof(Vendor), vendor.Id);

                foreach (var att in oldAttachments)
                {
                    await _fileService.DeleteFileAsync(att.FileUrl);
                    _unitOfWork.ProcurementAttachmentRepository.Delete(att);
                }

                if (request.Vendor.Attachments?.Any() == true)
                {
                    foreach (var attdto in request.Vendor.Attachments)
                    {
                        var path = await _fileService.SaveFileAsync(attdto.File, "vendorAttachments");

                        var newAttachment = new ProcurementAttachment
                        {
                            FileName = attdto.File.FileName,
                            FileUrl = path,
                            ContentType = attdto.File.ContentType,
                            FileSize = attdto.File.Length,
                            EntityType = nameof(Vendor),
                            EntityId = vendor.Id,
                            Description = attdto.Description,
                            UploadedAt = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow
                        };

                        await _unitOfWork.ProcurementAttachmentRepository.AddAsync(newAttachment);
                    }
                }
            }

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

            return new UpdateVendorCommandResponse
            {
                Success = true,
                Message = "Vendor updated successfully",
                Vendor = vendorDto
            };
        }
    }
}