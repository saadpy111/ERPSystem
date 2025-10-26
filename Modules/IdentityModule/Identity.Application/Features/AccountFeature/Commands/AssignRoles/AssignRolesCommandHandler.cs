using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.AccountFeature.Commands.AssignRoles
{
    public class AssignRolesCommandHandler : IRequestHandler<AssignRolesCommandRequest, AssignRolesCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AssignRolesCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AssignRolesCommandResponse> Handle(AssignRolesCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return new AssignRolesCommandResponse { Success = false, Errors = new List<string> { "User not found." } };

            // ensure roles exist
            var missing = new List<string>();
            foreach (var r in request.Roles.Distinct())
            {
                if (!await _roleManager.RoleExistsAsync(r)) missing.Add(r);
            }
            if (missing.Any())
            {
                return new AssignRolesCommandResponse
                {
                    Success = false,
                    Errors = new List<string> { $"These roles do not exist: {string.Join(", ", missing)}" }
                };
            }

            var result = await _userManager.AddToRolesAsync(user, request.Roles.Distinct());
            if (!result.Succeeded)
            {
                return new AssignRolesCommandResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            var assigned = (await _userManager.GetRolesAsync(user)).ToList();

            return new AssignRolesCommandResponse { Success = true, AssignedRoles = assigned };
        }
    }
}