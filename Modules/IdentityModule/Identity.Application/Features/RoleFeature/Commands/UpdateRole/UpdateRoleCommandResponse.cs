using Identity.Application.Dtos.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.RoleFeature.Commands.UpdateRole
{
    public class UpdateRoleCommandResponse
    {
        public bool Success { get; set; }
        public RoleDto? Role { get; set; }
        public List<string>? Errors { get; set; }
    }
}
