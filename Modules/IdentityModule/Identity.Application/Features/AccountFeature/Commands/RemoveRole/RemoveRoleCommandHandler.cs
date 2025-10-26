using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.AccountFeature.Commands.RemoveRole
{
    public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommandRequest, RemoveRoleCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RemoveRoleCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<RemoveRoleCommandResponse> Handle(RemoveRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return new RemoveRoleCommandResponse { Success = false, Errors = new List<string> { "User not found." } };

            if (!await _roleManager.RoleExistsAsync(request.Role))
                return new RemoveRoleCommandResponse { Success = false, Errors = new List<string> { "Role does not exist." } };

            var result = await _userManager.RemoveFromRoleAsync(user, request.Role);
            return new RemoveRoleCommandResponse
            {
                Success = result.Succeeded,
                Errors = result.Succeeded ? null : result.Errors.Select(e => e.Description).ToList()
            };
        }
    }
}