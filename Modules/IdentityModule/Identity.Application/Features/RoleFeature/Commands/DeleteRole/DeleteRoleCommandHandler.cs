using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.RoleFeature.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommandRequest, DeleteRoleCommandResponse>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DeleteRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<DeleteRoleCommandResponse> Handle(DeleteRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);
            if (role == null)
                return new DeleteRoleCommandResponse { Success = false, Errors = new List<string> { "Role not found." } };

            var result = await _roleManager.DeleteAsync(role);
            return new DeleteRoleCommandResponse
            {
                Success = result.Succeeded,
                Errors = result.Succeeded ? null : result.Errors.Select(e => e.Description).ToList()
            };
        }
    }
}
