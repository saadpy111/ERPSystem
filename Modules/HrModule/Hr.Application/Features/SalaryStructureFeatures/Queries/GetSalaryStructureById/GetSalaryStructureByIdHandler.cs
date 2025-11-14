using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.SalaryStructureFeatures.Queries.GetSalaryStructureById
{
    public class GetSalaryStructureByIdHandler : IRequestHandler<GetSalaryStructureByIdRequest, GetSalaryStructureByIdResponse>
    {
        private readonly ISalaryStructureRepository _salaryStructureRepository;
        private readonly IMapper _mapper;

        public GetSalaryStructureByIdHandler(ISalaryStructureRepository salaryStructureRepository, IMapper mapper)
        {
            _salaryStructureRepository = salaryStructureRepository;
            _mapper = mapper;
        }

        public async Task<GetSalaryStructureByIdResponse> Handle(GetSalaryStructureByIdRequest request, CancellationToken cancellationToken)
        {
            var salaryStructure = await _salaryStructureRepository.GetByIdAsync(request.Id);
            var salaryStructureDto = _mapper.Map<DTOs.SalaryStructureDto>(salaryStructure);

            return new GetSalaryStructureByIdResponse
            {
                SalaryStructure = salaryStructureDto
            };
        }
    }
}