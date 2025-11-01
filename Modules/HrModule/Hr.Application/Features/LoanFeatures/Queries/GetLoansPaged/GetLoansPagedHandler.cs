using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;

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
            var query = (await _repository.GetAllAsync()).AsQueryable();

            // Apply employee filter
            if (request.EmployeeId.HasValue)
            {
                query = query.Where(l => l.EmployeeId == request.EmployeeId.Value);
            }

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (Enum.TryParse<Hr.Domain.Enums.LoanStatus>(request.Status, true, out var status))
                {
                    query = query.Where(l => l.Status == status);
                }
            }

            var totalCount = query.Count();

            // Apply ordering
            query = ApplyOrdering(query, request.OrderBy, request.IsDescending);

            // Apply pagination
            var items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<LoanDto>>(items);

            return new GetLoansPagedResponse
            {
                PagedResult = new PagedResult<LoanDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.Loan> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.Loan> query,
            string? orderBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "StartDate";

            query = orderBy.ToLower() switch
            {
                "principalamount" => isDescending ? query.OrderByDescending(l => l.PrincipalAmount) : query.OrderBy(l => l.PrincipalAmount),
                "startdate" => isDescending ? query.OrderByDescending(l => l.StartDate) : query.OrderBy(l => l.StartDate),
                "remainingbalance" => isDescending ? query.OrderByDescending(l => l.RemainingBalance) : query.OrderBy(l => l.RemainingBalance),
                "status" => isDescending ? query.OrderByDescending(l => l.Status) : query.OrderBy(l => l.Status),
                _ => isDescending ? query.OrderByDescending(l => l.StartDate) : query.OrderBy(l => l.StartDate)
            };

            return query;
        }
    }
}
