using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hr.Application.Features.LoanInstallmentFeatures.GetLoanInstallmentsPaged
{
    public class GetLoanInstallmentsPagedHandler : IRequestHandler<GetLoanInstallmentsPagedRequest, GetLoanInstallmentsPagedResponse>
    {
        private readonly ILoanInstallmentRepository _repository;
        private readonly IMapper _mapper;

        public GetLoanInstallmentsPagedHandler(ILoanInstallmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetLoanInstallmentsPagedResponse> Handle(GetLoanInstallmentsPagedRequest request, CancellationToken cancellationToken)
        {
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.LoanId,
                request.Status,
                request.SearchTerm,
                request.OrderBy,
                request.IsDescending);

            var dtos = _mapper.Map<IEnumerable<LoanInstallmentDto>>(pagedResult.Items);

            return new GetLoanInstallmentsPagedResponse
            {
                PagedResult = new PagedResult<LoanInstallmentDto>
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