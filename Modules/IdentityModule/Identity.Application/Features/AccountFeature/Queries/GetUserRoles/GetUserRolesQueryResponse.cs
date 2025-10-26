namespace Identity.Application.Features.AccountFeature.Queries.GetUserRoles
{
    public class GetUserRolesQueryResponse
    {
        public List<string> Roles { get; set; } = new List<string>();
        public bool Found => Roles != null && Roles.Count > 0;
    }
}