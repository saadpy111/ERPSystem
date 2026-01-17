namespace Identity.Domain.Enums
{
    /// <summary>
    /// Defines the type of user in the system.
    /// This is separate from UserTenantState which tracks tenant relationship.
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// ERP system users (admin, managers, employees).
        /// Access the admin panel and ERP features.
        /// </summary>
        System = 1,
        
        /// <summary>
        /// Website customers (shoppers).
        /// Access the tenant's storefront to make orders.
        /// </summary>
        Client = 2
    }
}
