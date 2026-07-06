using Identity.Application.Dtos.AccountDtos;
using MediatR;

namespace Identity.Application.Features.AuthFeature.Queries.Login
{
    public class LoginQueryRequest : IRequest<LoginQueryResponse>
    {
        public  LoginDto  LoginDto { get; set; }
    }
}
