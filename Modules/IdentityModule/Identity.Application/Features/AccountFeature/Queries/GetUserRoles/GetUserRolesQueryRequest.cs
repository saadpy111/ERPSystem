using MediatR;

namespace Identity.Application.Features.AccountFeature.Queries.GetUserRoles
{
    public class GetUserRolesQueryRequest : IRequest<GetUserRolesQueryResponse>
    {
        public string UserId { get; set; } = string.Empty;
    }
}