using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _employeeRepository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.OrderBy,
                request.IsDescending,
                request.Status);

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(pagedResult.Items);

            return new GetEmployeesPagedResponse
            {
                PagedResult = new PagedResult<EmployeeDto>
                {
                    Items = employeeDtos,
                    TotalCount = pagedResult.TotalCount,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                }
            };
        }
    }
}