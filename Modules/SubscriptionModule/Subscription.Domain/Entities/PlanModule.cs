using System;

namespace Subscription.Domain.Entities
{
    public class PlanModule
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PlanId { get; set; } = string.Empty;
        public string ModuleId { get; set; } = string.Empty;

        [Obsolete("Use Module.Code via the Module navigation property instead. Will be removed in a future version.")]
        public string ModuleName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;

        public virtual SubscriptionPlan Plan { get; set; } = null!;
        public virtual Module Module { get; set; } = null!;
    }
}
