namespace Identity.Application.Features.AccountManagement.Commands.CreateUser
{
    public class CreateUserResponse
    {
        public bool Success { get; set; }
        public string? UserId { get; set; }
        public string? Error { get; set; }
    }
}
