using Hr.Application.DTOs;
using Hr.Application.Pagination;

namespace Hr.Application.Features.SalaryStructureFeatures.Queries.GetAllSalaryStructures
{
    public class GetSalaryStructuresPagedResponse
    {
        public PagedResult<SalaryStructurePagedDto> PagedResult { get; set; } = new PagedResult<SalaryStructurePagedDto>();
    }
}