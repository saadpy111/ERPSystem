using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetActiveEmployeeContracts
{
    public class GetActiveEmployeeContractsResponse
    {
        public IEnumerable<EmployeeContractDto> EmployeeContracts { get; set; } = new List<EmployeeContractDto>();
    }
}