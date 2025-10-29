using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Procurement.Application.DTOs
{
    public class CreatePurchaseOrderDto
    {
        [Required]
        public Guid VendorId { get; set; }
        
        public DateTime ExpectedDeliveryDate { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;
        
        [Required]
        public List<CreatePurchaseOrderItemDto> Items { get; set; } = new List<CreatePurchaseOrderItemDto>();
    }
    
    public class CreatePurchaseOrderItemDto
    {
        [Required]
        public Guid ProductId { get; set; }
        
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        
        public decimal UnitPrice { get; set; }
    }
}