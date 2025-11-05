using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Procurement.Application.DTOs.AttachmentDtos;

namespace Procurement.Application.DTOs
{
    public class UpdateVendorDto
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string ContactName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(50)]
        public string TaxNumber { get; set; } = string.Empty;

        public int? Rate { get; set; }
        [MaxLength(100)]
        public string? VendorCode { get; set; }
        [MaxLength(200)]
        public string? ProductVendorName { get; set; }
        [MaxLength(500)]
        public string? Webpage { get; set; }
        [MaxLength(100)]
        public string? Govrenment { get; set; }
        [MaxLength(100)]
        public string? City { get; set; }
        [MaxLength(50)]
        public string? Currency { get; set; }
        [MaxLength(100)]
        public string? PaymentMethod { get; set; }
        [MaxLength(100)]
        public string? CommercialRegistrationNumber { get; set; }
        public decimal? SupplierCreditLimit { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; }
        public bool EditAttachments { get; set; }
        public List<CreateAttachmentDto>? Attachments { get; set; }
    }
}