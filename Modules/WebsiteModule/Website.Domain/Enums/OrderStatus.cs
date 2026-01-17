namespace Website.Domain.Enums
{
    /// <summary>
    /// Status of an order in the fulfillment lifecycle.
    /// </summary>
    public enum OrderStatus
    {
        Pending = 0,
        Paid = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }
}
