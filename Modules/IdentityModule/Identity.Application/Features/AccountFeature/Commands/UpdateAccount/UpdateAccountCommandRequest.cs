using MediatR;

namespace Identity.Application.Features.AccountFeature.Commands.UpdateAccount
{
    public class UpdateAccountCommandRequest : IRequest<UpdateAccountCommandResponse>
    {
        public string Id { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? FullName { get; set; }
    }
}