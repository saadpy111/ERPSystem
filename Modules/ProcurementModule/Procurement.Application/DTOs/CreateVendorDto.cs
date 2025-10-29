using System.ComponentModel.DataAnnotations;

namespace Procurement.Application.DTOs
{
    public class CreateVendorDto
    {
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

        public bool IsActive { get; set; } = true;
    }
}