namespace Identity.Application.Features.AuthFeature.Commands.RegisterUser
{
    public class RegisterUserResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }
    }
}
