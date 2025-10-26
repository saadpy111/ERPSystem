using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Application.Dtos.AccountDtos;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.RoleFeature.Queries.GetRoleById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQueryRequest, GetRoleByIdQueryResponse>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GetRoleByIdQueryHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<GetRoleByIdQueryResponse> Handle(GetRoleByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);
            if (role == null) return new GetRoleByIdQueryResponse { Role = null };

            return new GetRoleByIdQueryResponse
            {
                Role = new RoleDto { Id = role.Id, Name = role.Name ?? string.Empty }
            };
        }
    }
}
