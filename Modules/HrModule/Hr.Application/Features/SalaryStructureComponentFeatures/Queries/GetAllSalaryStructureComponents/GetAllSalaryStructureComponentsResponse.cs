using Hr.Application.DTOs;
using System.Collections.Generic;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Queries.GetAllSalaryStructureComponents
{
    public class GetAllSalaryStructureComponentsResponse
    {
        public List<SalaryStructureComponentDto> SalaryStructureComponents { get; set; } = new List<SalaryStructureComponentDto>();
    }
}