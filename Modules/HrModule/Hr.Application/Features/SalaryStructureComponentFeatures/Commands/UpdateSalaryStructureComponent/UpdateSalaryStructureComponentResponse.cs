using Hr.Application.DTOs;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Commands.UpdateSalaryStructureComponent
{
    public class UpdateSalaryStructureComponentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public SalaryStructureComponentDto? SalaryStructureComponent { get; set; }
    }
}