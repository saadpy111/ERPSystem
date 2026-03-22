using Identity.Application.Contracts.Persistence;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.CreateUser
{
    /// <summary>
    /// Creates a new user with a specific UserType (System=ERP, Client=Website)
    /// and immediately assigns them to the caller's tenant.
    /// The UserType is enforced by the controller — this handler trusts it completely.
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IAuthRepository authRepository, IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Check email uniqueness (global — Identity requires unique emails)
            var existing = await _authRepository.FindByEmailAsync(request.Email);
            if (existing != null)
                return new CreateUserResponse { Success = false, Error = "Email is already in use." };

            var user = new ApplicationUser
            {
                Id            = Guid.NewGuid().ToString(),
                FullName      = request.FullName,
                Email         = request.Email,
                UserName      = request.Email,
                NormalizedEmail    = request.Email.ToUpperInvariant(),
                NormalizedUserName = request.Email.ToUpperInvariant(),
                UserType      = request.UserType,
                TenantId      = request.TenantId,
                State         = UserTenantState.TenantMember,
                TenantJoinedAt = DateTime.UtcNow
            };

            var result = await _authRepository.CreateUserAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new CreateUserResponse { Success = false, Error = errors };
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new CreateUserResponse { Success = true, UserId = user.Id };
        }
    }
}
