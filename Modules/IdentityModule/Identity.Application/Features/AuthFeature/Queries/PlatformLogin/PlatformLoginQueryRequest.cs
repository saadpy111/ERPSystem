using Identity.Application.Dtos.AccountDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.AuthFeature.Queries.PlatformLogin
{
    public class PlatformLoginQueryRequest : IRequest<PlatformLoginQueryResponse>
    {
        public LoginDto LoginDto { get; set; }

    }
}
