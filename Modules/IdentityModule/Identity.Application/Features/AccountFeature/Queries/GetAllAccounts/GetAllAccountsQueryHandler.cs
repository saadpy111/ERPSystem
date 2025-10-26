using Identity.Application.Contracts.Persistence;
using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Features.AccountFeature.Queries.GetAllAcounts;
using Identity.Application.Pagination;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQueryRequest, GetAllAccountsQueryResponse>
{
    private readonly IGenericRepository<ApplicationUser> _userRepository;

    public GetAllAccountsQueryHandler(IGenericRepository<ApplicationUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetAllAccountsQueryResponse> Handle(GetAllAccountsQueryRequest request, CancellationToken cancellationToken)
    {
        string search = request.Search?.Trim().ToLower() ?? "";

        var usersPaged = await _userRepository.Search(
            filter: u =>
                string.IsNullOrEmpty(search)
                || u.UserName.ToLower().Contains(search)
                || u.Email.ToLower().Contains(search)
                || (u.FullName != null && u.FullName.ToLower().Contains(search)),

            pagenom: request.PageNumber,
            pagesize: request.PageSize,
            null,
            q => q.Include(u => u.UserRoles)
               .ThenInclude(ur => ur.Role)
        );

        var dtoList = usersPaged.Items.Select(u => new AccountDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            FullName = u.FullName ?? "",
            Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
        }).ToList();

        var response = new GetAllAccountsQueryResponse
        {
            PagedResult = new PagedResult<AccountDto>
            {
                Items = dtoList,
                TotalCount = usersPaged.TotalCount
            }
        };

        return response;
    }
}