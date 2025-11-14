using Hr.Application.DTOs;

namespace Hr.Application.Features.SalaryStructureFeatures.Commands.CreateSalaryStructure
{
    public class CreateSalaryStructureResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public SalaryStructureDto? SalaryStructure { get; set; }
    }
}