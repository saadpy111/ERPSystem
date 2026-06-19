using Events.IdentityEvents;
using Identity.Application.Contracts.Persistence;
using Identity.Application.Contracts.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Multitenancy;
using SharedKernel.Website;
using System.Transactions;

namespace Identity.Application.Features.ClientAuthFeature.Commands.ClientRegister
{
    /// <summary>
    /// Handler for client (storefront customer) registration.
    /// 
    /// Flow:
    /// 1. Resolve tenant by domain (via WebsiteModule)
    /// 2. Validate domain is active/published
    /// 3. Create user with UserType = Client
    /// 4. Generate JWT token
    /// </summary>
    public class ClientRegisterCommandHandler : IRequestHandler<ClientRegisterCommand, ClientRegisterResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ITenantProvider _tenantProvider;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public ClientRegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            ITenantDomainResolver tenantDomainResolver,
            IJwtTokenService jwtTokenService,
            ITenantProvider tenantProvider,
            IMediator mediator,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _tenantProvider = tenantProvider;
             _mediator = mediator;
             _unitOfWork = unitOfWork;
        }

        public async Task<ClientRegisterResponse> Handle(ClientRegisterCommand request, CancellationToken cancellationToken)
        {
            // Validate phone number format
            if (string.IsNullOrWhiteSpace(request.PhoneNumber) || !IsValidPhoneNumber(request.PhoneNumber))
            {
                return Fail("A valid phone number is required.");
            }

            using var transaction = new TransactionScope(
                  TransactionScopeAsyncFlowOption.Enabled);
            try 
            {             
                // ===== STEP 1: Resolve tenant by domain =====
                var tenantId =  _tenantProvider.GetTenantId();
            
               if (string.IsNullOrEmpty(tenantId))
                return  Fail("Store not found. Please check the domain.");


                // ===== STEP 2: Validate email is unique within tenant =====
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null && existingUser.TenantId == tenantId)
                    return Fail("An account with this email already exists for this store.");
              
            
            

                // ===== STEP 3: Create client user =====
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = request.Email,
                    Email = request.Email,
                    FullName = request.FullName,
                    PhoneNumber = request.PhoneNumber,

                    // Client-specific settings
                    UserType = UserType.Client,
                    TenantId = tenantId,
                    State = UserTenantState.TenantMember,  // Clients are members of tenant
                    TenantJoinedAt = DateTime.UtcNow,

                    EmailConfirmed = true  // Auto-confirm for clients (can add email verification later)
                };

                var createResult = await _userManager.CreateAsync(user, request.Password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    return Fail($"Registration failed: {errors}");
                }

                // ===== STEP 4: Generate JWT token =====
                // Clients have minimal permissions - just basic access
                var roles = new List<string>();  // Clients typically don't have roles
                var permissions = new List<string>();

                await _mediator.Publish(new ClientRegisteredEvent
                {
                    UserId = user.Id,
                    TenantId = user.TenantId!,
                    Email = user.Email!,
                    FullName = user.FullName!,
                    PhoneNumber = user.PhoneNumber
                });
                transaction.Complete();

                var token = _jwtTokenService.GenerateToken(user, roles, permissions, tenantId);

                return new ClientRegisterResponse
                {
                    Success = true,
                    UserId = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    TenantId = tenantId,
                    PhoneNumber = user.PhoneNumber,
                    Token = token
                };
            }
            catch
            {
                return new ClientRegisterResponse()
                {
                    Success = false
    
                };
            }
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());
            return digitsOnly.Length >= 7 && digitsOnly.Length <= 15;
        }

        private static ClientRegisterResponse Fail(string error)
        {
            return new ClientRegisterResponse { Success = false, Error = error };
        }
    }
}
