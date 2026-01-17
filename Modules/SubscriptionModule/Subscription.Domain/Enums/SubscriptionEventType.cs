namespace Subscription.Domain.Enums
{
    public enum SubscriptionEventType
    {
        Created = 0,
        Upgraded = 1,
        Downgraded = 2,
        Renewed = 3,
        Suspended = 4,
        Reactivated = 5,
        Canceled = 6,
        Expired = 7,
        TrialStarted = 8,
        TrialConverted = 9
    }
}
