using System;

namespace Subscription.Domain.Entities
{
    /// <summary>
    /// Junction table defining which modules are enabled in each subscription plan.
    /// Plans enable modules, modules define permissions.
    /// </summary>
    public class PlanModule
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PlanId { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty; // "Inventory", "HR", "CRM", "Accounting", "Ecommerce"
        public bool IsEnabled { get; set; } = true;
        
        // Navigation
        public virtual SubscriptionPlan Plan { get; set; } = null!;
    }
}
