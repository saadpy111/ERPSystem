using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeesPaged
{
    public class GetEmployeesPagedHandler : IRequestHandler<GetEmployeesPagedRequest, GetEmployeesPagedResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeesPagedHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<GetEmployeesPagedResponse> Handle(GetEmployeesPagedRequest request, CancellationToken cancellationToken)
        {
            var query = (await _employeeRepository.GetAllAsync()).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(e => 
                    e.FullName.ToLower().Contains(searchTerm) ||
                    e.Email.ToLower().Contains(searchTerm) ||
                    (e.PhoneNumber != null && e.PhoneNumber.ToLower().Contains(searchTerm)));
            }

            // Apply department filter
            // This is now handled through EmployeeContract

            // Apply status filter
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                if (Enum.TryParse<Hr.Domain.Enums.EmployeeStatus>(request.Status, true, out var status))
                {
                    query = query.Where(e => e.Status == status);
                }
            }

            // Get total count before pagination
            var totalCount = query.Count();

            // Apply ordering
            query = ApplyOrdering(query, request.OrderBy, request.IsDescending);

            // Apply pagination
            var employees = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return new GetEmployeesPagedResponse
            {
                PagedResult = new PagedResult<EmployeeDto>
                {
                    Items = employeeDtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }

        private IQueryable<Hr.Domain.Entities.Employee> ApplyOrdering(
            IQueryable<Hr.Domain.Entities.Employee> query, 
            string? orderBy, 
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "FullName";

            query = orderBy.ToLower() switch
            {
                "fullname" => isDescending ? query.OrderByDescending(e => e.FullName) : query.OrderBy(e => e.FullName),
                "email" => isDescending ? query.OrderByDescending(e => e.Email) : query.OrderBy(e => e.Email),
                "status" => isDescending ? query.OrderByDescending(e => e.Status) : query.OrderBy(e => e.Status),
                _ => isDescending ? query.OrderByDescending(e => e.FullName) : query.OrderBy(e => e.FullName)
            };

            return query;
        }
    }
}
