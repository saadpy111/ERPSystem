using Hr.Application.DTOs;
using Hr.Application.Pagination;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetEmployeeContractsPaged
{
    public class GetEmployeeContractsPagedResponse
    {
        public PagedResult<EmployeeContractDto> PagedResult { get; set; } = new PagedResult<EmployeeContractDto>();
    }
}