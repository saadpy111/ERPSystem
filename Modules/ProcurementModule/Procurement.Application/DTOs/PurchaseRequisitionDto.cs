using System;
using System.ComponentModel.DataAnnotations;

namespace Procurement.Application.DTOs
{
    public class PurchaseRequisitionDto
    {
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string RequestedBy { get; set; } = string.Empty;
        
        public DateTime RequestDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}