namespace Website.Domain.Enums
{
    /// <summary>
    /// Payment methods supported for checkout.
    /// </summary>
    public enum PaymentMethod
    {
        CreditCard = 0,
        COD = 1,          // Cash on Delivery
        BankTransfer = 2
    }
}
