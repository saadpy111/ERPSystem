using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.AccountFeature.Queries.GetAllAcounts
{
    public class GetAllAccountsQueryRequest : IRequest<GetAllAccountsQueryResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
    }
}
