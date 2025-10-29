using System;
using System.ComponentModel.DataAnnotations;

namespace Procurement.Application.DTOs
{
    public class UpdatePurchaseOrderDto
    {
        [Required]
        public Guid Id { get; set; }
        
        public Guid? VendorId { get; set; }
        
        public DateTime? ExpectedDeliveryDate { get; set; }
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        [MaxLength(50)]
        public string? Status { get; set; }
    }
}