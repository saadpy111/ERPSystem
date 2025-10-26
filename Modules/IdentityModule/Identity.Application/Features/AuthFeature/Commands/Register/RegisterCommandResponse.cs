namespace Identity.Application.Features.AuthFeature.Commands.Register
{
    public class RegisterCommandResponse
    {
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }
    }
}