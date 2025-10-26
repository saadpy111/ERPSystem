namespace Identity.Application.Features.AccountFeature.Commands.RemoveRole
{
    public class RemoveRoleCommandResponse
    {
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }
    }
}