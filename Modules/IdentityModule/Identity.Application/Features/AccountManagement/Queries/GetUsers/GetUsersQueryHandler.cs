using Identity.Application.Contracts.Persistence;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
    {
        private readonly IAuthRepository _authRepository;

        public GetUsersQueryHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var paged = await _authRepository.GetUsersPagedAsync(
                request.TenantId,
                request.UserType,
                request.Search,
                request.PageNumber,
                request.PageSize);

            return new GetUsersResponse { Result = paged };
        }
    }
}
