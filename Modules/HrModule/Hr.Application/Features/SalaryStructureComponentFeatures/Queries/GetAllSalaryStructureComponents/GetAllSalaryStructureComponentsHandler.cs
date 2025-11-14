using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Queries.GetAllSalaryStructureComponents
{
    public class GetAllSalaryStructureComponentsHandler : IRequestHandler<GetAllSalaryStructureComponentsRequest, GetAllSalaryStructureComponentsResponse>
    {
        private readonly ISalaryStructureComponentRepository _salaryStructureComponentRepository;
        private readonly IMapper _mapper;

        public GetAllSalaryStructureComponentsHandler(ISalaryStructureComponentRepository salaryStructureComponentRepository, IMapper mapper)
        {
            _salaryStructureComponentRepository = salaryStructureComponentRepository;
            _mapper = mapper;
        }

        public async Task<GetAllSalaryStructureComponentsResponse> Handle(GetAllSalaryStructureComponentsRequest request, CancellationToken cancellationToken)
        {
            var salaryStructureComponents = await _salaryStructureComponentRepository.GetAllAsync();
            var salaryStructureComponentDtos = _mapper.Map<List<DTOs.SalaryStructureComponentDto>>(salaryStructureComponents);

            return new GetAllSalaryStructureComponentsResponse
            {
                SalaryStructureComponents = salaryStructureComponentDtos
            };
        }
    }
}