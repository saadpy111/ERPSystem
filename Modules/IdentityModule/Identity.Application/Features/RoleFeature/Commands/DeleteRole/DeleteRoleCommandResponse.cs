using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.RoleFeature.Commands.DeleteRole
{
    public class DeleteRoleCommandResponse
    {
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }
    }
}
