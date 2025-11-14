using Hr.Application.DTOs;

namespace Hr.Application.Features.SalaryStructureFeatures.Commands.UpdateSalaryStructure
{
    public class UpdateSalaryStructureResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public SalaryStructureDto? SalaryStructure { get; set; }
    }
}