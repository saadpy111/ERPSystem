using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.AccountFeature.Commands.DeleteAccount
{
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommandRequest, DeleteAccountCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteAccountCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<DeleteAccountCommandResponse> Handle(DeleteAccountCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null) return new DeleteAccountCommandResponse { Success = false, Errors = new List<string> { "User not found." } };

            var result = await _userManager.DeleteAsync(user);
            return new DeleteAccountCommandResponse
            {
                Success = result.Succeeded,
                Errors = result.Succeeded ? null : result.Errors.Select(e => e.Description).ToList()
            };
        }
    }
}