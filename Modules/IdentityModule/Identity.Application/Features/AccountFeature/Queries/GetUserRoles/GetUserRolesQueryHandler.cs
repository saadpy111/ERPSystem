using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.AccountFeature.Queries.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQueryRequest, GetUserRolesQueryResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserRolesQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetUserRolesQueryResponse> Handle(GetUserRolesQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return new GetUserRolesQueryResponse { Roles = new List<string>() };

            var roles = await _userManager.GetRolesAsync(user);
            return new GetUserRolesQueryResponse { Roles = roles.ToList() };
        }
    }
}