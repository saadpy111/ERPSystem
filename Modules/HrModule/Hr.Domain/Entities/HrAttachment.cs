using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Domain.Entities
{
    public class HrAttachment:BaseEntity
    {
        public int Id { get; set; }
        
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string? ContentType { get; set; }
        public long? FileSize { get; set; }
        // Polymorphic association
        public string EntityType { get; set; } 
        public int EntityId { get; set; } // The PK of the associated entity
        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
