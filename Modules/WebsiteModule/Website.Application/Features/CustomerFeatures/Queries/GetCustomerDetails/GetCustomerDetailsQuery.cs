using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.CustomerFeatures.Queries.GetCustomerDetails
{
    public class GetCustomerDetailsQuery : IRequest<CustomerDetailsDto?>
    {
        public Guid CustomerId { get; set; }
        public int OrdersPage { get; set; } = 1;
        public int OrdersPageSize { get; set; } = 10;
    }
}
