using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.EmployeeContractFeatures.Queries.GetEmployeeContractsPaged
{
    public class GetEmployeeContractsPagedHandler : IRequestHandler<GetEmployeeContractsPagedRequest, GetEmployeeContractsPagedResponse>
    {
        private readonly IEmployeeContractRepository _repository;
        private readonly IMapper _mapper;

        public GetEmployeeContractsPagedHandler(IEmployeeContractRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetEmployeeContractsPagedResponse> Handle(GetEmployeeContractsPagedRequest request, CancellationToken cancellationToken)
        {
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.EmployeeId,
                request.JobId,
                request.ContractType,
                request.OrderBy,
                request.IsDescending);

            var dtos = _mapper.Map<IEnumerable<EmployeeContractDto>>(pagedResult.Items);

            return new GetEmployeeContractsPagedResponse
            {
                PagedResult = new PagedResult<EmployeeContractDto>
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