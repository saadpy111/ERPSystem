using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.SalaryStructureFeatures.Queries.GetAllSalaryStructures
{
    public class GetSalaryStructuresPagedHandler : IRequestHandler<GetSalaryStructuresPagedRequest, GetSalaryStructuresPagedResponse>
    {
        private readonly ISalaryStructureRepository _salaryStructureRepository;
        private readonly IMapper _mapper;

        public GetSalaryStructuresPagedHandler(ISalaryStructureRepository salaryStructureRepository, IMapper mapper)
        {
            _salaryStructureRepository = salaryStructureRepository;
            _mapper = mapper;
        }

        public async Task<GetSalaryStructuresPagedResponse> Handle(GetSalaryStructuresPagedRequest request, CancellationToken cancellationToken)
        {
            var pagedResult = await _salaryStructureRepository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.OrderBy,
                request.IsDescending);

            var dtos = _mapper.Map<IEnumerable<SalaryStructurePagedDto>>(pagedResult.Items);

            return new GetSalaryStructuresPagedResponse
            {
                PagedResult = new PagedResult<SalaryStructurePagedDto>
                {
                    Items = dtos,
                    TotalCount = pagedResult.TotalCount,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                }
            };
        }
    }
}