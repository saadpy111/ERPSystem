using Identity.Application.Dtos.AccountDtos;
using MediatR;


namespace Identity.Application.Features.AuthFeature.Commands.Register
{
    public class RegisterCommandRequest : IRequest<RegisterCommandResponse>
    {
        public RegisterDto   RegisterDto{ get; set; }
    }
}