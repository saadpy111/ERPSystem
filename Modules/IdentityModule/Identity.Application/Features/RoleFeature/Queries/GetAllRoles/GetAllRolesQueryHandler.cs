using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Pagination;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace Identity.Application.Features.RoleFeature.Queries.GetAllRoles
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQueryRequest, GetAllRolesQueryResponse>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GetAllRolesQueryHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<GetAllRolesQueryResponse> Handle(GetAllRolesQueryRequest request, CancellationToken cancellationToken)
        {
            var search = request.Search?.Trim().ToLower() ?? string.Empty;
            var query = _roleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(r => r.Name != null && r.Name.ToLower().Contains(search));
            }

            var total = await query.CountAsync(cancellationToken);
            var page = Math.Max(1, request.PageNumber);
            var size = Math.Clamp(request.PageSize, 1, 1000);

            var items = await query
                .OrderBy(r => r.Name)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(r => new RoleDto { Id = r.Id, Name = r.Name ?? string.Empty })
                .ToListAsync(cancellationToken);

            return new GetAllRolesQueryResponse
            {
                PagedResult = new PagedResult<RoleDto>
                {
                    Items = items,
                    TotalCount = total
                }
            };
        }
    }
}