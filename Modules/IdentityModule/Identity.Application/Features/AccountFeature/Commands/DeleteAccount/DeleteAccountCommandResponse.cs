namespace Identity.Application.Features.AccountFeature.Commands.DeleteAccount
{
    public class DeleteAccountCommandResponse
    {
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }
    }
}