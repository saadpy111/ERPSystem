using MediatR;

namespace Identity.Application.Features.AccountFeature.Commands.DeleteAccount
{
    public class DeleteAccountCommandRequest : IRequest<DeleteAccountCommandResponse>
    {
        public string Id { get; set; } = string.Empty;
    }
}