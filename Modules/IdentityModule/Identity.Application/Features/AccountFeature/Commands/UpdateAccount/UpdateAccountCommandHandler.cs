using Identity.Application.Dtos.AccountDtos;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.AccountFeature.Commands.UpdateAccount
{
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommandRequest, UpdateAccountCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateAccountCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UpdateAccountCommandResponse> Handle(UpdateAccountCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return new UpdateAccountCommandResponse
                {
                    Success = false,
                    Errors = new List<string> { "User not found." }
                };
            }

            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
                user.Email = request.Email;

            if (request.FullName is not null)
                user.FullName = request.FullName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new UpdateAccountCommandResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UpdateAccountCommandResponse
            {
                Success = true,
                Account = new AccountDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? "",
                    Email = user.Email ?? "",
                    FullName = user.FullName ?? "",
                    Roles = roles.ToList()
                }
            };
        }
    }
}