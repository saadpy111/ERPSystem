using Identity.Application.Dtos.AccountDtos;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.RoleFeature.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandResponse>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<CreateRoleCommandResponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new CreateRoleCommandResponse { Success = false, Errors = new List<string> { "Role name is required." } };
            }

            if (await _roleManager.RoleExistsAsync(request.Name))
            {
                return new CreateRoleCommandResponse { Success = false, Errors = new List<string> { "Role already exists." } };
            }

            var role = new ApplicationRole { Name = request.Name.Trim()  , TenantId = "36324700-44c0-4748-9b12-df7125c0e03a" };
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return new CreateRoleCommandResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new CreateRoleCommandResponse
            {
                Success = true,
                Role = new RoleDto { Id = role.Id, Name = role.Name ?? string.Empty }
            };
        }
    }
}
