using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using Identity.Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Application.Features.AuthFeature.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommandRequest, RegisterCommandResponse>
    {
        private readonly IAuthRepository _registrationService;

        public RegisterCommandHandler(IAuthRepository registrationService)
        {
            _registrationService = registrationService;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.RegisterDto.Email,
                Email = request.RegisterDto.Email,
                FullName = request.RegisterDto.FullName
            };

            var result = await _registrationService.CreateUserAsync(user, request.RegisterDto.Password);
            if (!result.Succeeded)

                return new RegisterCommandResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };

       

            return new RegisterCommandResponse { Success = true };
        }
    }
}
