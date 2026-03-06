using MediatR;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.CustomerFeatures.Queries.GetCustomersPaged
{
    public class GetCustomersPagedQuery : IRequest<PagedResult<CustomerListDto>>
    {
        public CustomerFilter Filter { get; set; } = new();
    }
}
