using AutoMapper;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using Hr.Application.Pagination;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            // Use repository-level pagination instead of handler-level pagination
            var pagedResult = await _departmentRepository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.OrderBy,
                request.IsDescending);

            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(pagedResult.Items);

            return new GetDepartmentsPagedResponse
            {
                PagedResult = new PagedResult<DepartmentDto>
                {
                    Items = departmentDtos,
                    TotalCount = pagedResult.TotalCount,
                    PageNumber = pagedResult.PageNumber,
                    PageSize = pagedResult.PageSize
                }
            };
        }
    }
}