using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.RoleFeature.Commands.DeleteRole
{
    public class DeleteRoleCommandRequest : IRequest<DeleteRoleCommandResponse>
    {
        public string Id { get; set; } = string.Empty;
    }
}
