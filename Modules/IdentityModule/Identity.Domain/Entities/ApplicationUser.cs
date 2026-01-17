using Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;


namespace Identity.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        
        /// <summary>
        /// Type of user: System (ERP users) or Client (website customers)
        /// </summary>
        public UserType UserType { get; set; } = UserType.System;
        
        /// <summary>
        /// Nullable TenantId - NULL when user is in PendingTenant state
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// User's tenant lifecycle state
        /// </summary>
        public UserTenantState State { get; set; } = UserTenantState.PendingTenant;

        /// <summary>
        /// When the user joined/created a tenant (NULL if PendingTenant)
        /// </summary>
        public DateTime? TenantJoinedAt { get; set; }

        // Navigation properties
        public virtual Tenant? Tenant { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}

