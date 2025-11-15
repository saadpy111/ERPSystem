using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.PayrollComponentFeatures.GetPayrollComponentsPaged
{
    public class GetPayrollComponentsPagedHandler : IRequestHandler<GetPayrollComponentsPagedRequest, GetPayrollComponentsPagedResponse>
    {
        private readonly IPayrollComponentRepository _repository;
        private readonly IMapper _mapper;

        public GetPayrollComponentsPagedHandler(IPayrollComponentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetPayrollComponentsPagedResponse> Handle(GetPayrollComponentsPagedRequest request, CancellationToken cancellationToken)
        {
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.PayrollRecordId,
                request.ComponentType,
                request.OrderBy,
                request.IsDescending);

            var dtos = _mapper.Map<IEnumerable<PayrollComponentDto>>(pagedResult.Items);

            return new GetPayrollComponentsPagedResponse
            {
                PagedResult = new PagedResult<PayrollComponentDto>
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