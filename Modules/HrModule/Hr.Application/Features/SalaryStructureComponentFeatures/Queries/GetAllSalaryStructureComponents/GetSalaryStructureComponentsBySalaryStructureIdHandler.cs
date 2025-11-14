using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Queries.GetAllSalaryStructureComponents
{
    public class GetSalaryStructureComponentsBySalaryStructureIdHandler : IRequestHandler<GetSalaryStructureComponentsBySalaryStructureIdRequest, GetAllSalaryStructureComponentsResponse>
    {
        private readonly ISalaryStructureComponentRepository _salaryStructureComponentRepository;
        private readonly IMapper _mapper;

        public GetSalaryStructureComponentsBySalaryStructureIdHandler(ISalaryStructureComponentRepository salaryStructureComponentRepository, IMapper mapper)
        {
            _salaryStructureComponentRepository = salaryStructureComponentRepository;
            _mapper = mapper;
        }

        public async Task<GetAllSalaryStructureComponentsResponse> Handle(GetSalaryStructureComponentsBySalaryStructureIdRequest request, CancellationToken cancellationToken)
        {
            var salaryStructureComponents = await _salaryStructureComponentRepository.GetBySalaryStructureIdAsync(request.SalaryStructureId);
            var salaryStructureComponentDtos = _mapper.Map<List<DTOs.SalaryStructureComponentDto>>(salaryStructureComponents);

            return new GetAllSalaryStructureComponentsResponse
            {
                SalaryStructureComponents = salaryStructureComponentDtos
            };
        }
    }
}