using MediatR;
using System.Collections.Generic;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Queries.GetAllSalaryStructureComponents
{
    public class GetSalaryStructureComponentsBySalaryStructureIdRequest : IRequest<GetAllSalaryStructureComponentsResponse>
    {
        public int SalaryStructureId { get; set; }
    }
}