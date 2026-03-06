using MediatR;
using SharedKernel.Contracts;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.CustomerFeatures.Queries.GetCustomersPaged
{
    public class GetCustomersPagedQueryHandler
        : IRequestHandler<GetCustomersPagedQuery, PagedResult<CustomerListDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserLookupService _userLookupService;

        public GetCustomersPagedQueryHandler(
            ICustomerRepository customerRepository,
            IUserLookupService userLookupService)
        {
            _customerRepository = customerRepository;
            _userLookupService = userLookupService;
        }

        public async Task<PagedResult<CustomerListDto>> Handle(
            GetCustomersPagedQuery request,
            CancellationToken cancellationToken)
        {
            List<string>? searchUserIds = null;
            if (!string.IsNullOrWhiteSpace(request.Filter.Search))
            {
                searchUserIds = await _userLookupService.SearchUserIdsByTermAsync(request.Filter.Search, cancellationToken);
                
                // If search term was provided but no users found, return empty results immediately
                if (!searchUserIds.Any())
                {
                    return new PagedResult<CustomerListDto>
                    {
                        Items = new List<CustomerListDto>(),
                        TotalCount = 0,
                        Page = request.Filter.PageNumber,
                        PageSize = request.Filter.PageSize
                    };
                }
            }

            PagedResult<CustomerListDto>? result = await _customerRepository.GetCustomersPagedAsync(
                request.Filter,
                searchUserIds,
                cancellationToken);

            if (result.Items.Count > 0)
            {
                var userIds = result.Items.Select(c => c.UserId).ToList();
                var users = await _userLookupService.GetUsersByIdsAsync(userIds, cancellationToken);
                var userMap = users.ToDictionary(u => u.UserId);

                foreach (var item in result.Items)
                {
                    if (userMap.TryGetValue(item.UserId, out var user))
                    {
                        item.FullName = user.FullName;
                        item.Email = user.Email;
                        item.PhoneNumber = user.PhoneNumber;
                    }
                }
            }

            return result;
        }
    }
}
