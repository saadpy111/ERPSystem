using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Subscription;
using SharedKernel.Constants;
using SharedKernel.Website;

namespace Identity.Application.Features.TenantFeature.Commands.CreateCompany
{
    /// <summary>
    /// Handler for creating a new company (tenant).
    /// 
    /// CROSS-MODULE BOUNDARIES:
    /// - IdentityModule: Tenant creation, User assignment, Roles
    /// - SubscriptionModule: Subscription creation (via ISubscriptionService)
    /// - WebsiteModule: Website configuration (via IWebsiteProvisioningService)
    /// 
    /// IdentityModule does NOT:
    /// - Load themes
    /// - Apply themes
    /// - Build SiteConfig
    /// </summary>
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyResponse>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IWebsiteProvisioningService _websiteProvisioningService;
        private readonly IWebsiteImageService _websiteImageService;

        public CreateCompanyCommandHandler(
            IAuthRepository authRepository,
            ITenantRepository tenantRepository,
            IRolePermissionRepository rolePermissionRepository,
            RoleManager<ApplicationRole> roleManager,
            IPermissionRepository permissionRepository,
            IJwtTokenService jwtTokenService,
            IUnitOfWork unitOfWork,
            ISubscriptionService subscriptionService,
            IWebsiteProvisioningService websiteProvisioningService,
            IWebsiteImageService websiteImageService)
        {
            _authRepository = authRepository;
            _tenantRepository = tenantRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _roleManager = roleManager;
            _permissionRepository = permissionRepository;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
            _subscriptionService = subscriptionService;
            _websiteProvisioningService = websiteProvisioningService;
            _websiteImageService = websiteImageService;
        }

        public async Task<CreateCompanyResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
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
                return new CreateCompanyResponse { Success = false, Error = "Subscription plan selection is required" };

            if (string.IsNullOrWhiteSpace(request.CurrencyCode))
                return new CreateCompanyResponse { Success = false, Error = "Currency selection is required" };


            // ===== STEP 1: CREATE TENANT (IdentityModule responsibility) =====
            
            var tenant = await _tenantRepository.CreateAsync(new Tenant
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.CompanyName,
                Code = request.CompanyCode.ToUpper(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

            // ===== STEP 2: ASSIGN USER TO TENANT =====
            
            user.TenantId = tenant.Id;
            user.State = UserTenantState.TenantOwner;
            user.TenantJoinedAt = DateTime.UtcNow;
            await _authRepository.UpdateAsync(user);

            // ===== STEP 3: CREATE DEFAULT ROLES =====
            
            var defaultRoles = new[] 
            { 
                Roles.SuperAdmin, 
                Roles.InventoryManager, 
                Roles.HRManager, 
                Roles.ProcurementManager, 
                Roles.ReportViewer ,
                Roles.WebsiteAdmin
            };
            var createdRoles = new List<ApplicationRole>();

            foreach (var roleName in defaultRoles)
            {
                var tenantRoleName = $"{roleName}_{tenant.Code}";
                var role = new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = tenantRoleName,
                    NormalizedName = tenantRoleName.ToUpper(),
                    TenantId = tenant.Id
                };

                var roleResult = await _roleManager.CreateAsync(role);
                if (!roleResult.Succeeded)
                {
                    return new CreateCompanyResponse
                    {
                        Success = false,
                        Error = $"Failed to create role {roleName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}"
                    };
                }
                createdRoles.Add(role);
            }

            // ===== STEP 4: CREATE SUBSCRIPTION (via SubscriptionModule) =====
            
            var subscriptionResult = await _subscriptionService.CreateSubscriptionAsync(
                tenant.Id,
                tenant.Name,
                request.PlanCode,
                request.CurrencyCode,
                request.Interval
            );

            if (!subscriptionResult.Success)
            {
                return new CreateCompanyResponse
                {
                    Success = false,
                    Error = subscriptionResult.Error ?? "Failed to create subscription"
                };
            }

            // ===== STEP 5: ASSIGN PERMISSIONS TO ROLES =====
            
            await AssignPermissionsToRolesBasedOnModules(tenant.Id, createdRoles, subscriptionResult.EnabledModules);

            // ===== STEP 6: ASSIGN SUPERADMIN ROLE TO USER =====
            
            var superAdminRole = createdRoles.FirstOrDefault(r => r.Name == $"{Roles.SuperAdmin}_{tenant.Code}");
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

            // ===== COMMIT IDENTITY CHANGES =====
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // ===== STEP 7: PROCESS WEBSITE IMAGES (via WebsiteModule) =====
            // Delegate file handling to WebsiteModule before initializing website
            
            string logoUrl = string.Empty;
            string heroBackgroundImageUrl = string.Empty;

            if (request.Logo != null)
            {
                logoUrl = await _websiteImageService.ProcessWebsiteLogoAsync(tenant.Id, request.Logo);
            }

            if (string.IsNullOrEmpty(request.ThemeCode) && request.HeroBackgroundImage != null)
            {
                heroBackgroundImageUrl = await _websiteImageService.ProcessWebsiteHeroImageAsync(tenant.Id, request.HeroBackgroundImage);
            }

            // ===== STEP 8: INITIALIZE WEBSITE (via WebsiteModule) =====
            // This is done AFTER tenant is committed and images are processed
            
            var websiteRequest = new WebsiteInitializationRequest
            {
                // Theme selection (determines mode)
                ThemeCode = request.ThemeCode,
                
                // Business data (always from user)
                SiteName = request.SiteName,
                Domain = request.Domain,
                BusinessType = request.BusinessType,
                LogoUrl = logoUrl,
                about_the_site = request.about_the_site,
                location = request.location,
                phone = request.phone,
                email = request.email,
                
                // Presentation data (used ONLY in Custom mode, IGNORED in Theme mode)
                // Map from flattened command properties
                Colors = string.IsNullOrEmpty(request.PrimaryColor) ? null : new WebsiteColors
                {
                    Primary = request.PrimaryColor,
                    Secondary = request.SecondaryColor ?? string.Empty,
                    Background = request.BackgroundColor ?? string.Empty,
                    Text = request.TextColor ?? string.Empty
                },
                Hero = string.IsNullOrEmpty(request.HeroTitle) ? null : new WebsiteHero
                {
                    Title = request.HeroTitle,
                    Subtitle = request.HeroSubtitle ?? string.Empty,
                    ButtonText = request.HeroButtonText ?? string.Empty,
                    BackgroundImage = heroBackgroundImageUrl // Use generated path
                },
                Sections = request.Sections
            };

            var websiteResult = await _websiteProvisioningService.InitializeTenantWebsiteAsync(
                tenant.Id,
                websiteRequest
            );

            if (!websiteResult.Success)
            {
                // Website provisioning failed - return error but tenant is already created
                // In production, you might want to log this or handle differently
                return new CreateCompanyResponse
                {
                    Success = false,
                    Error = websiteResult.Error ?? "Failed to initialize website"
                };
            }

            // ===== STEP 9: GENERATE NEW JWT TOKEN =====
            
            var permissions = await _permissionRepository.GetUserEffectivePermissionsAsync(user.Id);
            var roles = new List<string> { $"{Roles.SuperAdmin}_{tenant.Code}" };
            var newToken = _jwtTokenService.GenerateToken(user, roles, permissions, tenant.Id);

            return new CreateCompanyResponse
            {
                Success = true,
                TenantId = tenant.Id,
                TenantName = tenant.Name,
                NewToken = newToken,
                SubscriptionPlanName = subscriptionResult.PlanName,
                IsTrial = subscriptionResult.IsTrial,
                TrialEndsAt = subscriptionResult.TrialEndsAt
            };
        }

        private async Task AssignPermissionsToRolesBasedOnModules(
            string tenantId, 
            List<ApplicationRole> roles, 
            List<string> enabledModules)
        {
            var allPermissions = await _rolePermissionRepository.GetAllPermissionsAsync();
            var enabledPermissions = allPermissions
                .Where(p => string.IsNullOrEmpty(p.Module) || enabledModules.Contains(p.Module))
                .ToList();

            foreach (var role in roles)
            {
                List<string> permissionIdsToAssign = new List<string>();

                if (role.Name!.Contains(Roles.SuperAdmin))
                {
                    permissionIdsToAssign = enabledPermissions.Select(p => p.Id).ToList();
                }
                else if (role.Name.Contains(Roles.InventoryManager))
                {
                    permissionIdsToAssign = enabledPermissions.Where(p => p.Module == "Inventory").Select(p => p.Id).ToList();
                }
                else if (role.Name.Contains(Roles.HRManager))
                {
                    permissionIdsToAssign = enabledPermissions.Where(p => p.Module == "HR").Select(p => p.Id).ToList();
                }
                else if (role.Name.Contains(Roles.ProcurementManager))
                {
                    permissionIdsToAssign = enabledPermissions.Where(p => p.Module == "Procurement").Select(p => p.Id).ToList();
                }
                else if (role.Name.Contains(Roles.ReportViewer))
                {
                    permissionIdsToAssign = enabledPermissions.Where(p => p.Module == "Report").Select(p => p.Id).ToList();
                }
                else if(role.Name.Contains(Roles.WebsiteAdmin))
                {
                    permissionIdsToAssign = enabledPermissions.Where(p => p.Module == "Website").Select(p => p.Id).ToList();
                }


                if (permissionIdsToAssign.Any())
                {
                    await _rolePermissionRepository.AssignPermissionsToRoleAsync(role.Id, permissionIdsToAssign, tenantId);
                }
            }
        }
    }
}
