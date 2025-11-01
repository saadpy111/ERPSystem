using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;

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
            var query = (await _repository.GetAllAsync()).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(pc => pc.Name.ToLower().Contains(searchTerm));
            }

            // Apply payroll record filter
            if (request.PayrollRecordId.HasValue)
            {
                query = query.Where(pc => pc.PayrollRecordId == request.PayrollRecordId.Value);
            }

            // Apply component type filter
            if (!string.IsNullOrWhiteSpace(request.ComponentType))
            {
                if (Enum.TryParse<Hr.Domain.Enums.PayrollComponentType>(request.ComponentType, true, out var componentType))
                {
                    query = query.Where(pc => pc.ComponentType == componentType);
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

            var dtos = _mapper.Map<IEnumerable<PayrollComponentDto>>(items);

            return new GetPayrollComponentsPagedResponse
            {
                PagedResult = new PagedResult<PayrollComponentDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.PayrollComponent> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.PayrollComponent> query,
            string? orderBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Name";

            query = orderBy.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(pc => pc.Name) : query.OrderBy(pc => pc.Name),
                "amount" => isDescending ? query.OrderByDescending(pc => pc.Amount) : query.OrderBy(pc => pc.Amount),
                "componenttype" => isDescending ? query.OrderByDescending(pc => pc.ComponentType) : query.OrderBy(pc => pc.ComponentType),
                _ => isDescending ? query.OrderByDescending(pc => pc.Name) : query.OrderBy(pc => pc.Name)
            };

            return query;
        }
    }
}
