using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Pagination;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Queries.GetRoles
{
    public class GetRolesQuery : IRequest<GetRolesResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public RoleScope Scope { get; set; }
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetRolesResponse
    {
        public PagedResult<RoleDto> Result { get; set; } = new();
    }
}
