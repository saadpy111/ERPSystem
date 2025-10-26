using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Identity.Application.Features.AccountFeature.Queries.GetAccountById
{
    public class GetAccountByIdQueryRequest : IRequest<GetAccountByIdQueryResponse>
    {
        public string Id { get; set; } = string.Empty;
    }
}
