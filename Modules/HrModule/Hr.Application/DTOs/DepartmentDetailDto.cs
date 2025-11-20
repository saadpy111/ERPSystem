using System;
using System.Collections.Generic;

namespace Hr.Application.DTOs
{
    public class DepartmentDetailDto
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentDepartmentId { get; set; }
        public string? ParentDepartmentName { get; set; }
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public ICollection<DepartmentDetailDto> SubDepartments { get; set; } = new List<DepartmentDetailDto>();
        public ICollection<JobDto> Jobs { get; set; } = new List<JobDto>();
        public ICollection<HrAttachmentDto> Attachments { get; set; } = new List<HrAttachmentDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}