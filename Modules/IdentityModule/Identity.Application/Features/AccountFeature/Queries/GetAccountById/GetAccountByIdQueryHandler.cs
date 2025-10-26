using Identity.Application.Dtos.AccountDtos;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.AccountFeature.Queries.GetAccountById
{
    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQueryRequest, GetAccountByIdQueryResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAccountByIdQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetAccountByIdQueryResponse> Handle(GetAccountByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null) return new GetAccountByIdQueryResponse { Account = null };

            var roles = await _userManager.GetRolesAsync(user);

            var dto = new AccountDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                FullName = user.FullName ?? "",
                Roles = roles.ToList()
            };

            return new GetAccountByIdQueryResponse { Account = dto };
        }
    }
}