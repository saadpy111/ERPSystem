using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Application.Dtos.AccountDtos;

namespace Identity.Application.Features.RoleFeature.Commands.CreateRole
{
    public class CreateRoleCommandResponse
    {
        public bool Success { get; set; }
        public RoleDto? Role { get; set; }
        public List<string>? Errors { get; set; }
    }
}
