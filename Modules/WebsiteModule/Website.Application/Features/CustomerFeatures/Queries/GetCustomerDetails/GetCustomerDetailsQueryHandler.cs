using MediatR;
using SharedKernel.Contracts;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.CustomerFeatures.Queries.GetCustomerDetails
{
    public class GetCustomerDetailsQueryHandler
        : IRequestHandler<GetCustomerDetailsQuery, CustomerDetailsDto?>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserLookupService _userLookupService;

        public GetCustomerDetailsQueryHandler(
            ICustomerRepository customerRepository,
            IUserLookupService userLookupService)
        {
            _customerRepository = customerRepository;
            _userLookupService = userLookupService;
        }

        public async Task<CustomerDetailsDto?> Handle(
            GetCustomerDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var dto = await _customerRepository.GetCustomerDetailsAsync(
                request.CustomerId,
                request.OrdersPage,
                request.OrdersPageSize,
                cancellationToken);

            if (dto == null) return null;

            // Enrich profile fields from Identity module
            var user = await _userLookupService.GetUserByIdAsync(dto.UserId, cancellationToken);
            if (user != null)
            {
                dto.FullName = user.FullName;
                dto.Email = user.Email;
                dto.PhoneNumber = user.PhoneNumber;
            }

            return dto;
        }
    }
}
