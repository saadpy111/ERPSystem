using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.GetDepartmentsPaged
{
    public class GetDepartmentsPagedHandler : IRequestHandler<GetDepartmentsPagedRequest, GetDepartmentsPagedResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public GetDepartmentsPagedHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<GetDepartmentsPagedResponse> Handle(GetDepartmentsPagedRequest request, CancellationToken cancellationToken)
        {
            var query = (await _departmentRepository.GetAllAsync()).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(d => 
                    d.Name.ToLower().Contains(searchTerm) ||
                    (d.Description != null && d.Description.ToLower().Contains(searchTerm)));
            }

            var totalCount = query.Count();

            // Apply ordering
            query = ApplyOrdering(query, request.OrderBy, request.IsDescending);

            // Apply pagination
            var departments = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);

            return new GetDepartmentsPagedResponse
            {
                PagedResult = new PagedResult<DepartmentDto>
                {
                    Items = departmentDtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.Department> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.Department> query,
            string? orderBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "Name";

            query = orderBy.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
                _ => isDescending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name)
            };

            return query;
        }
    }
}
