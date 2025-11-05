using System;
using System.Collections.Generic;
using Procurement.Application.DTOs.AttachmentDtos;

namespace Procurement.Application.DTOs
{
    public class VendorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public int? Rate { get; set; }
        public string? VendorCode { get; set; }
        public string? ProductVendorName { get; set; }
        public string? Webpage { get; set; }
        public string? Govrenment { get; set; }
        public string? City { get; set; }
        public string? Currency { get; set; }
        public string? PaymentMethod { get; set; }
        public string? CommercialRegistrationNumber { get; set; }
        public decimal? SupplierCreditLimit { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AttachmentDto>? Attachments { get; set; }
    }
}