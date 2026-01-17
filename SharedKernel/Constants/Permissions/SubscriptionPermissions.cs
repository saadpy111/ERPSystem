namespace SharedKernel.Constants.Permissions
{
    /// <summary>
    /// Subscription Module Permissions
    /// Based on actual endpoints from Subscription.Api controllers
    /// </summary>
    public static class SubscriptionPermissions
    {
        public const string Module = "Subscription";

        // ===== PLANS =====
        public const string PlansView = "Subscription.Plans.View";
        public const string PlansCreate = "Subscription.Plans.Create";
        public const string PlansEdit = "Subscription.Plans.Edit";
        public const string PlansDelete = "Subscription.Plans.Delete";

        // ===== SUBSCRIPTIONS =====
        public const string SubscriptionsView = "Subscription.Subscriptions.View";
        public const string SubscriptionsCreate = "Subscription.Subscriptions.Create";
        public const string SubscriptionsCancel = "Subscription.Subscriptions.Cancel";
        public const string SubscriptionsUpgrade = "Subscription.Subscriptions.Upgrade";
        public const string SubscriptionsDowngrade = "Subscription.Subscriptions.Downgrade";
    }
}
