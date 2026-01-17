namespace Identity.Domain.Enums
{
    /// <summary>
    /// Represents the tenant lifecycle state of a user
    /// </summary>
    public enum UserTenantState
    {
        /// <summary>
        /// User registered but has not created or joined a tenant yet
        /// </summary>
        PendingTenant = 0,

        /// <summary>
        /// User created a company and is the tenant owner
        /// </summary>
        TenantOwner = 1,

        /// <summary>
        /// User joined an existing company via invitation
        /// </summary>
        TenantMember = 2
    }
}
