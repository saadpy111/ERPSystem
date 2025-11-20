namespace Hr.Application.DTOs
{
    public class EmployeeContractDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int? SalaryStructureId { get; set; }
        public string? SalaryStructureName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Salary { get; set; }
        public string ContractType { get; set; } = string.Empty;
        public int? ProbationPeriod { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public ICollection<HrAttachmentDto> Attachments { get; set; } = new List<HrAttachmentDto>();
        public ICollection<SalaryStructureComponentDto>  salaryStructureComponentDtos { get; set; } = new List<SalaryStructureComponentDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}