using System;
using System.Collections.Generic;

namespace Inventory.Domain.Entities
{
    public class Attachment : BaseEntity
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string? ContentType { get; set; }
        public long? FileSize { get; set; }
        // Polymorphic association
        public string EntityType { get; set; } // e.g., "Product", "Warehouse", etc.
        public Guid EntityId { get; set; } // The PK of the associated entity
        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
