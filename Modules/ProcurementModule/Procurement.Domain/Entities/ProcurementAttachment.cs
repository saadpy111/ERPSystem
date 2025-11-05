using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procurement.Domain.Entities
{
    public class ProcurementAttachment : BaseEntity
    {
        [MaxLength(200)]
        public string FileName { get; set; } = string.Empty;
        [MaxLength(200)]

        public string FileUrl { get; set; } = string.Empty;
        [MaxLength(200)]

        public string? ContentType { get; set; }
        public long? FileSize { get; set; }
        // Polymorphic association
        [MaxLength(200)]

        public string EntityType { get; set; } = string.Empty; // e.g., "Vendor", "PurchaseOrder", etc.
        public Guid EntityId { get; set; } // The PK of the associated entity
        [MaxLength(250)]

        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
