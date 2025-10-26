using Identity.Application.Dtos.AccountDtos;

namespace Identity.Application.Features.AccountFeature.Queries.GetAccountById
{
    public class GetAccountByIdQueryResponse
    {
        public AccountDto? Account { get; set; }
        public bool Found => Account != null;
    }
}