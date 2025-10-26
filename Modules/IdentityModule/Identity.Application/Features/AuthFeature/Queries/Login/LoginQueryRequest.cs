using Identity.Application.Dtos.AccountDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.AuthFeature.Queries.Login
{
    public class LoginQueryRequest : IRequest<LoginQueryResponse>
    {
        public  LoginDto  LoginDto { get; set; }
    }
}
