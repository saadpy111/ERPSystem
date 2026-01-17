namespace Subscription.Domain.Enums
{
    public enum SubscriptionStatus
    {
        Trial = 0,      // Free trial period
        Active = 1,     // Paid & active
        Suspended = 2,  // Payment failed, grace period
        Expired = 3,    // Trial ended or canceled
        Canceled = 4    // User canceled, ends at period end
    }
}
