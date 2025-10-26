using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.RoleFeature.Queries.GetAllRoles
{
    public class GetAllRolesQueryRequest : IRequest<GetAllRolesQueryResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string? Search { get; set; }
    }
}
