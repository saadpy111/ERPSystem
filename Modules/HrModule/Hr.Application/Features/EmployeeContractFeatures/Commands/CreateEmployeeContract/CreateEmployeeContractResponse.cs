using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeContractFeatures.Commands.CreateEmployeeContract
{
    public class CreateEmployeeContractResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public EmployeeContractDto? EmployeeContract { get; set; }
    }
}