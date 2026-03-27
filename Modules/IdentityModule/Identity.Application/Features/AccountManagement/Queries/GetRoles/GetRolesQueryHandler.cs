using Identity.Application.Contracts.Persistence;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Queries.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, GetRolesResponse>
    {
        private readonly IAuthRepository _authRepository;

        public GetRolesQueryHandler(IAuthRepository authRepository)
            => _authRepository = authRepository;

        public async Task<GetRolesResponse> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var paged = await _authRepository.GetRolesPagedAsync(
                request.TenantId, request.Scope, request.Search, request.PageNumber, request.PageSize);

            return new GetRolesResponse { Result = paged };
        }
    }
}
