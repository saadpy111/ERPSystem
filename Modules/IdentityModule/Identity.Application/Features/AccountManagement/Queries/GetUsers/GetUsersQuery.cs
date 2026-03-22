using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Pagination;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<GetUsersResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public UserType UserType { get; set; }
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetUsersResponse
    {
        public PagedResult<AccountDto> Result { get; set; } = new();
    }
}
