using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Pagination;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features.AccountFeature.Queries.GetAllAcounts
{
    public class GetAllAccountsQueryResponse
    {
        public PagedResult<AccountDto>  PagedResult { get; set; }
    }
}
