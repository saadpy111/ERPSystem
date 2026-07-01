using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Features.TenantFeature.Commands.CreateCompany
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SharedKernel.Subscription.ISubscriptionService _subscriptionService;
        private readonly ITenantRoleProvisioningService _tenantRoleProvisioningService;

        public CreateCompanyCommandHandler(
            IAuthRepository authRepository,
            ITenantRepository tenantRepository,
            RoleManager<ApplicationRole> roleManager,
            IPermissionRepository permissionRepository,
            IJwtTokenService jwtTokenService,
            IUnitOfWork unitOfWork,
            SharedKernel.Subscription.ISubscriptionService subscriptionService,
            ITenantRoleProvisioningService tenantRoleProvisioningService)
        {
            _authRepository = authRepository;
            _tenantRepository = tenantRepository;
            _roleManager = roleManager;
            _permissionRepository = permissionRepository;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
            _subscriptionService = subscriptionService;
            _tenantRoleProvisioningService = tenantRoleProvisioningService;
        }

        public async Task<CreateCompanyResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // ===== VALIDATION =====
                var user = await _authRepository.FindByIdAsync(request.UserId);
                if (user == null)
                    return new CreateCompanyResponse { Success = false, Error = "User not found." };

                if (user.TenantId != null)
                    return new CreateCompanyResponse { Success = false, Error = "User already belongs to a company." };

                if (user.State != UserTenantState.PendingTenant)
                    return new CreateCompanyResponse { Success = false, Error = "User is not in pending tenant state." };

                if (await _tenantRepository.ExistsAsync(request.CompanyCode))
                    return new CreateCompanyResponse { Success = false, Error = "Company code already exists." };

                if (string.IsNullOrWhiteSpace(request.PlanCode))
                    return new CreateCompanyResponse { Success = false, Error = "Subscription plan selection is required." };

                if (string.IsNullOrWhiteSpace(request.CurrencyCode))
                    return new CreateCompanyResponse { Success = false, Error = "Currency selection is required." };

                // ===== CREATE TENANT =====
                var tenant = await _tenantRepository.CreateAsync(new Tenant
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.CompanyName,
                    Code = request.CompanyCode.ToUpper(),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // ===== ASSIGN USER TO TENANT =====
                user.TenantId = tenant.Id;
                user.State = UserTenantState.TenantOwner;
                user.TenantJoinedAt = DateTime.UtcNow;
                await _authRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // ===== CREATE SUBSCRIPTION =====
                var subscriptionResult = await _subscriptionService.CreateSubscriptionAsync(
                    tenant.Id,
                    tenant.Name,
                    request.PlanCode,
                    request.CurrencyCode,
                    request.Interval);

                if (!subscriptionResult.Success)
                {
                    return new CreateCompanyResponse
                    {
                        Success = false,
                        Error = subscriptionResult.Error ?? "Failed to create subscription."
                    };
                }

                // ===== PROVISION ROLES + SYNC PERMISSIONS =====
                var provisionResult = await _tenantRoleProvisioningService.ProvisionRolesAsync(
                    tenant.Id,
                    subscriptionResult.EnabledModules);

                if (!provisionResult.Success)
                {
                    return new CreateCompanyResponse
                    {
                        Success = false,
                        Error = provisionResult.Error
                    };
                }

                // ===== ASSIGN SUPERADMIN ROLE TO USER =====
                var superAdminRoleName = $"SuperAdmin_{tenant.Id}";
                var superAdminRole = _roleManager.Roles
                    .FirstOrDefault(r => r.Name == superAdminRoleName && r.TenantId == tenant.Id);

                if (superAdminRole != null)
                {
                    var userRole = new ApplicationUserRole
                    {
                        UserId = user.Id,
                        RoleId = superAdminRole.Id,
                        TenantId = tenant.Id,
                        AssignedAt = DateTime.UtcNow,
                        AssignedBy = user.Id
                    };
                    await _authRepository.AddUserRoleAsync(userRole);
                }

                // ===== COMMIT =====
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                // ===== GENERATE JWT (final step, after commit) =====
                var tenantId = user.TenantId ?? string.Empty;
                var uiPermissions = string.IsNullOrEmpty(tenantId)
                    ? new List<string>()
                    : await _permissionRepository.GetUserEffectivePermissionsAsync(user.Id, tenantId);

                var roles = _roleManager.Roles
                    .Where(r => r.TenantId == tenant.Id)
                    .Select(r => r.Name!)
                    .ToList();

                var newToken = _jwtTokenService.GenerateToken(user, roles, new List<string>(), tenant.Id);

                return new CreateCompanyResponse
                {
                    Success = true,
                    TenantId = tenant.Id,
                    TenantName = tenant.Name,
                    NewToken = newToken,
                    SubscriptionPlanName = subscriptionResult.PlanName,
                    IsTrial = subscriptionResult.IsTrial,
                    TrialEndsAt = subscriptionResult.TrialEndsAt,
                    Roles = roles.Select(r => r.ToCleanRoleName(tenant.Id)).ToList(),
                    Permissions = uiPermissions
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
