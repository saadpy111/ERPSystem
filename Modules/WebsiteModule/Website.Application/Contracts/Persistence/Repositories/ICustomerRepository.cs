using Website.Application.DTOs;
using Website.Application.Pagination;
using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence.Repositories
{
    public interface ICustomerRepository
    {
        Task CreateAsync(CustomerProfile profile, CancellationToken cancellationToken = default);

        Task<PagedResult<CustomerListDto>> GetCustomersPagedAsync(
            CustomerFilter filter,
            IEnumerable<string>? userIds,
            CancellationToken cancellationToken);

        Task<CustomerDetailsDto?> GetCustomerDetailsAsync(
            Guid customerId,
            int ordersPage,
            int ordersPageSize,
            CancellationToken cancellationToken);
    }
}
