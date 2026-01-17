using Identity.Domain.Enums;

namespace Identity.Domain.Entities
{
    /// <summary>
    /// Represents an invitation to join a tenant (company)
    /// </summary>
    public class TenantInvitation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Tenant (company) the user is invited to
        /// </summary>
        public string TenantId { get; set; } = string.Empty;
        
        /// <summary>
        /// Email address of the invited user
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Role to assign when invitation is accepted
        /// </summary>
        public string RoleId { get; set; } = string.Empty;
        
        /// <summary>
        /// User who sent the invitation
        /// </summary>
        public string InvitedBy { get; set; } = string.Empty;
        
        /// <summary>
        /// Current status of the invitation
        /// </summary>
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
        
        /// <summary>
        /// Unique token for accepting the invitation
        /// </summary>
        public string InvitationToken { get; set; } = string.Empty;
        
        /// <summary>
        /// When the invitation expires
        /// </summary>
        public DateTime ExpiresAt { get; set; }
        
        /// <summary>
        /// When the invitation was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// When the invitation was accepted (NULL if not accepted)
        /// </summary>
        public DateTime? AcceptedAt { get; set; }
        
        /// <summary>
        /// User who accepted the invitation (NULL if not accepted)
        /// </summary>
        public string? AcceptedBy { get; set; }
        
        // Navigation properties
        public virtual Tenant Tenant { get; set; } = null!;
        public virtual ApplicationRole Role { get; set; } = null!;
        public virtual ApplicationUser InvitedByUser { get; set; } = null!;
        public virtual ApplicationUser? AcceptedByUser { get; set; }
    }
}
