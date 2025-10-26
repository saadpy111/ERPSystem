using Identity.Application.Dtos.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.RoleFeature.Queries.GetRoleById
{
    public class GetRoleByIdQueryResponse
    {
        public RoleDto? Role { get; set; }
    }
}
