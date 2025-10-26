
using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using MediatR;

namespace Identity.Application.Features.AuthFeature.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQueryRequest, LoginQueryResponse>
    {
        private readonly IAuthRepository _authService;
        private readonly IJwtTokenService _jwt;

        public LoginQueryHandler(IAuthRepository authService, IJwtTokenService jwt)
        {
            _authService = authService;
            _jwt = jwt;
        }

        public async Task<LoginQueryResponse> Handle(LoginQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _authService.FindByEmailAsync(request.LoginDto.Email);
            if (user == null)
                return new LoginQueryResponse { Success = false, Error = "Invalid credentials" };

            var passwordValid = await _authService.CheckPasswordAsync(user, request.LoginDto.Password);
            if (!passwordValid)
                return new LoginQueryResponse { Success = false, Error = "Invalid credentials" };

            var roles = await _authService.GetUserRolesAsync(user);
            var token = _jwt.GenerateToken(user, roles);

            return new LoginQueryResponse { Success = true, Token = token };
        }
    }
}
