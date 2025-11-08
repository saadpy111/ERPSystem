using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetEmployeeContractsByEmployeeId
{
    public class GetEmployeeContractsByEmployeeIdResponse
    {
        public IEnumerable<EmployeeContractDto> EmployeeContracts { get; set; } = new List<EmployeeContractDto>();
    }
}