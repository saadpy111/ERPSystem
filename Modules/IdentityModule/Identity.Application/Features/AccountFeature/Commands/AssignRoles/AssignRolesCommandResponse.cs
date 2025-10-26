namespace Identity.Application.Features.AccountFeature.Commands.AssignRoles
{
    public class AssignRolesCommandResponse
    {
        public bool Success { get; set; }
        public List<string>? AssignedRoles { get; set; }
        public List<string>? Errors { get; set; }
    }
}