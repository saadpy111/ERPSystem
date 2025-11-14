using System;
using System.Collections.Generic;

namespace Hr.Application.DTOs
{
    public class SalaryStructurePagedDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int ComponentCount { get; set; }
    }
}