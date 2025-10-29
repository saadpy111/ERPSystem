using System;
using System.ComponentModel.DataAnnotations;

namespace Procurement.Application.DTOs
{
    public class CreatePurchaseRequisitionDto
    {
        [Required]
        [MaxLength(100)]
        public string RequestedBy { get; set; } = string.Empty;
        
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}