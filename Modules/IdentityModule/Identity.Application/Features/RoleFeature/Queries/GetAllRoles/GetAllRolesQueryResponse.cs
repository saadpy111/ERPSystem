using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.RoleFeature.Queries.GetAllRoles
{
    public class GetAllRolesQueryResponse
    {
        public PagedResult<RoleDto> PagedResult { get; set; } = new PagedResult<RoleDto>();
    }
}
