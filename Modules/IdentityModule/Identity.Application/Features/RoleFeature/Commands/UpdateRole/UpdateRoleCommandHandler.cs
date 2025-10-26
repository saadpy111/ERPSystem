using Identity.Application.Dtos.AccountDtos;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Application.Features.RoleFeature.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCommandResponse>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UpdateRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);
            if (role == null)
            {
                return new UpdateRoleCommandResponse { Success = false, Errors = new List<string> { "Role not found." } };
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new UpdateRoleCommandResponse { Success = false, Errors = new List<string> { "Role name is required." } };
            }

            role.Name = request.Name.Trim();
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                return new UpdateRoleCommandResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new UpdateRoleCommandResponse
            {
                Success = true,
                Role = new RoleDto { Id = role.Id, Name = role.Name ?? string.Empty }
            };
        }
    }
}
