using Identity.Application.Dtos.AccountDtos;

namespace Identity.Application.Features.AccountFeature.Commands.UpdateAccount
{
    public class UpdateAccountCommandResponse
    {
        public bool Success { get; set; }
        public AccountDto? Account { get; set; }
        public List<string>? Errors { get; set; }
    }
}