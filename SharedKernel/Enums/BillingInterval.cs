namespace SharedKernel.Enums
{
    /// <summary>
    /// Billing interval for subscription pricing.
    /// Used across modules (Subscription, Identity) to maintain consistency.
    /// </summary>
    public enum BillingInterval
    {
        OneTime = 0,
        Monthly = 1,
        Quarterly = 3,
        Yearly = 12
    }
}
