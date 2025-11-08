using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetAllEmployeeContracts
{
    public class GetAllEmployeeContractsResponse
    {
        public IEnumerable<EmployeeContractDto> EmployeeContracts { get; set; } = new List<EmployeeContractDto>();
    }
}