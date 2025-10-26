using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.RoleFeature.Queries.GetRoleById
{
    public class GetRoleByIdQueryRequest : IRequest<GetRoleByIdQueryResponse>
    {
        public string Id { get; set; } = string.Empty;
    }
}
