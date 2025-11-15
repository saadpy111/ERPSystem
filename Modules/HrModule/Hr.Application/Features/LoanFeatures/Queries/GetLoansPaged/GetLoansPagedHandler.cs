using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.LoanFeatures.GetLoansPaged
{
    public class GetLoansPagedHandler : IRequestHandler<GetLoansPagedRequest, GetLoansPagedResponse>
    {
        private readonly ILoanRepository _repository;
        private readonly IMapper _mapper;

        public GetLoansPagedHandler(ILoanRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetLoansPagedResponse> Handle(GetLoansPagedRequest request, CancellationToken cancellationToken)
        {
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.EmployeeId,
                request.Status,
                request.OrderBy,
                request.IsDescending);

            var dtos = _mapper.Map<IEnumerable<LoanDto>>(pagedResult.Items);

            return new GetLoansPagedResponse
            {
                PagedResult = new PagedResult<LoanDto>
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