using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.SalaryStructureComponentFeatures.Queries.GetSalaryStructureComponentById
{
    public class GetSalaryStructureComponentByIdHandler : IRequestHandler<GetSalaryStructureComponentByIdRequest, GetSalaryStructureComponentByIdResponse>
    {
        private readonly ISalaryStructureComponentRepository _salaryStructureComponentRepository;
        private readonly IMapper _mapper;

        public GetSalaryStructureComponentByIdHandler(ISalaryStructureComponentRepository salaryStructureComponentRepository, IMapper mapper)
        {
            _salaryStructureComponentRepository = salaryStructureComponentRepository;
            _mapper = mapper;
        }

        public async Task<GetSalaryStructureComponentByIdResponse> Handle(GetSalaryStructureComponentByIdRequest request, CancellationToken cancellationToken)
        {
            var salaryStructureComponent = await _salaryStructureComponentRepository.GetByIdAsync(request.Id);
            var salaryStructureComponentDto = _mapper.Map<DTOs.SalaryStructureComponentDto>(salaryStructureComponent);

            return new GetSalaryStructureComponentByIdResponse
            {
                SalaryStructureComponent = salaryStructureComponentDto
            };
        }
    }
}